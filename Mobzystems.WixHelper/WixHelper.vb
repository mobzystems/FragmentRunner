Imports System.Xml

Module WixHelper
  Private AllProjects As New Dictionary(Of String, Project)(StringComparer.OrdinalIgnoreCase)

  Class FileInfo
    Public Name As String
    ' Public Path As String
    Public Source As String

    'Public Sub New(name As String, path As String, source As String)
    '  Me.Name = name
    '  ' Me.Path = path
    '  Me.Source = source
    'End Sub

    Public Sub New(path As String, source As String)
      Me.Name = path
      ' Me.Path = path
      Me.Source = source
    End Sub

    Public ReadOnly Property Id As String
      Get
        Return Me.Name.Replace("\", ".").Replace(" ", "_")
      End Get
    End Property
  End Class

  Class Project
    ''' <summary>
    ''' Full path name of the project file
    ''' </summary>
    Public Filename As String
    ''' <summary>
    ''' Name of the project = file name of the project file
    ''' </summary>
    Public ProjectName As String

    Public AssemblyName As String
    Public OutputType As String

    Public ContentFiles As New List(Of FileInfo)
    Public SourceFiles As New List(Of FileInfo)
    Public OutputFiles As New List(Of FileInfo)

    Public SourceProjects As New List(Of Project)

    Public Shortcut As FileInfo

    Protected xml As XmlDocument
    Protected nsm As XmlNamespaceManager

    Public Sub New(projectFile As String)
      ' Console.WriteLine("WixHelper: reading file " + projectFile)

      xml = New XmlDocument
      nsm = New XmlNamespaceManager(xml.NameTable)
      nsm.AddNamespace("x", "http://schemas.microsoft.com/developer/msbuild/2003")
      xml.Load(projectFile)

      Me.Filename = IO.Path.GetFullPath(projectFile)
      Me.ProjectName = IO.Path.GetFileNameWithoutExtension(projectFile)

      If Not AllProjects.ContainsKey(Me.Filename) Then
        AllProjects.Add(Me.Filename, Me)
      End If

      Me.OutputType = SelectText("/x:Project/x:PropertyGroup/x:OutputType")
      Me.AssemblyName = SelectText("/x:Project/x:PropertyGroup/x:AssemblyName")

      Dim contentNodes = SelectNodes("/x:Project/x:ItemGroup/x:Content")
      For Each contentNode As XmlElement In contentNodes
        Me.ContentFiles.Add(New FileInfo(contentNode.Attributes("Include").Value, "$(var." + ProjectName + ".ProjectDir)"))
      Next

      Dim sourceNodes = SelectNodes("/x:Project/x:ItemGroup/x:Compile")
      For Each sourceNode As XmlElement In sourceNodes
        Dim f As New FileInfo(sourceNode.Attributes("Include").Value, "$(var." + ProjectName + ".ProjectDir)")
        Me.SourceFiles.Add(f)
      Next

      Dim projectRefNodes = SelectNodes("/x:Project/x:ItemGroup/x:ProjectReference")
      For Each refNode As XmlElement In projectRefNodes
        Me.SourceProjects.Add(New Project(refNode.Attributes("Include").Value))
      Next

      Select Case Me.OutputType
        Case "Exe", "WinExe"
          Me.Shortcut = New FileInfo(Me.AssemblyName + ".exe", "$(var." + Me.ProjectName + ".TargetDir)")
        Case "Library"
          Me.Shortcut = New FileInfo(Me.AssemblyName + ".dll", "$(var." + Me.ProjectName + ".TargetDir)")
        Case Else
          Me.Shortcut = Nothing
      End Select

      If Me.Shortcut IsNot Nothing Then
        Me.OutputFiles.Add(Me.Shortcut)
        Dim hasAppConfig As Boolean = SelectNodes("/x:Project/x:ItemGroup/x:None[translate(@Include, 'AP.CONFIG', 'ap.config')='app.config']").Count > 0

        If hasAppConfig Then
          Me.OutputFiles.Add(New FileInfo(Me.Shortcut.Name + ".config", "$(var." + ProjectName + ".TargetDir)"))
        End If
      End If

      ' Console.WriteLine("WixHelper: file was read - " + projectFile)
    End Sub

    Protected Function SelectNode(query As String) As XmlNode
      Return Me.xml.SelectSingleNode(query, Me.nsm)
    End Function

    Protected Function SelectText(query As String) As String
      Dim n As XmlNode = Me.SelectNode(query)
      If n Is Nothing Then
        Return Nothing
      Else
        Return n.InnerText
      End If
    End Function

    Protected Function SelectNodes(query As String) As XmlNodeList
      Return Me.xml.SelectNodes(query, Me.nsm)
    End Function

    Public Function CreateComponentGroup(projectName As String, componentName As String, files As IEnumerable(Of FileInfo), shortcutFile As FileInfo) As XElement
      Dim cg = <ComponentGroup Id=<%= projectName + "." + componentName %>/>
      For Each file In files
        cg.Add(<ComponentRef Id=<%= projectName + "." + componentName + "." + file.Id %>/>)
      Next

      If shortcutFile IsNot Nothing Then
        cg.Add(<ComponentRef Id=<%= projectName + "." + componentName + "Folder" %>/>)
      End If

      Return cg
    End Function

    Public Function CreateDirectoryRef(projectName As String, componentName As String, files As IEnumerable(Of FileInfo), shortcutFile As FileInfo) As XElement
      Dim dr = <DirectoryRef Id=<%= projectName + "." + componentName %>/>
      For Each file In files
        Dim c = <Component Id=<%= projectName + "." + componentName + "." + file.Id %>/>
        dr.Add(c)
        Dim f = <File Id=<%= projectName + "." + componentName + "." + file.Id %> Source=<%= file.Source + "\" + file.Name %> KeyPath="yes"/>
        c.Add(f)
        If file Is shortcutFile Then
          f.Add(<Shortcut Id=<%= projectName + "." + componentName + "." + file.Id %> Name=<%= projectName %> Directory=<%= projectName + "." + componentName + "Folder" %> WorkingDirectory=<%= projectName + ".Output" %> Advertise="yes" IconIndex="0" Icon=<%= file.Name %>>
                  <Icon Id=<%= file.Name %> SourceFile=<%= file.Source + "\" + file.Name %>/>
                </Shortcut>)
        End If
      Next
      Return dr
    End Function

    Public Sub WriteWixIncludeFile(outputFilename As String, name As String, componentName As String, files As IEnumerable(Of FileInfo), Optional writeShortcut As Boolean = False)
      Dim x = <?xml version="1.0"?>
              <Include>
              </Include>
      x.Root.Add(CreateDirectoryRef(name, componentName, files, If(writeShortcut, Me.Shortcut, Nothing)))
      If writeShortcut Then
        x.Root.Add(
        <DirectoryRef Id=<%= ProjectName + "." + componentName + "Folder" %>>
          <Component Id=<%= ProjectName + "." + componentName + "Folder" %>>
            <RemoveFolder Id=<%= ProjectName + "." + componentName + "Folder" %> On="uninstall"/>
            <RegistryValue Root="HKMU" Key=<%= "Software\[Manufacturer]\Install\$(var.PRODUCTNAME)\" + ProjectName + "." + componentName %> Type="string" Value="" KeyPath="yes"/>
          </Component>
        </DirectoryRef>)
      End If
      x.Root.Add(CreateComponentGroup(name, componentName, files, If(writeShortcut, Me.Shortcut, Nothing)))

      x.Save(outputFilename)
    End Sub
  End Class

  Function Main(args() As String) As Integer
    Dim projectFile As String = args(0)
    Dim outputFolder As String = args(1)

    Console.WriteLine("WixHelper: generating dependencies for " + projectFile + " into " + outputFolder)
    Try
      Dim myProject As New Project(projectFile)

      Dim includeXml As New XmlDocument
      includeXml.AppendChild(includeXml.CreateXmlDeclaration("1.0", "utf-8", Nothing))
      Dim rootNode As XmlElement = includeXml.CreateElement("Include")

      For Each p In AllProjects.Values
        If p IsNot myProject Then
          rootNode.AppendChild(includeXml.CreateProcessingInstruction("include", p.ProjectName + ".wxi"))
          p.WriteWixIncludeFile(IO.Path.Combine(outputFolder, p.ProjectName + ".Content.wxi"), p.ProjectName, "Content", p.ContentFiles)
          p.WriteWixIncludeFile(IO.Path.Combine(outputFolder, p.ProjectName + ".Output.wxi"), p.ProjectName, "Output", p.OutputFiles)
          p.WriteWixIncludeFile(IO.Path.Combine(outputFolder, p.ProjectName + ".Shortcut.wxi"), p.ProjectName, "Shortcut", p.OutputFiles, True)
          p.WriteWixIncludeFile(IO.Path.Combine(outputFolder, p.ProjectName + ".Source.wxi"), p.ProjectName, "Source", p.SourceFiles)
        End If
      Next

      includeXml.AppendChild(rootNode)

      includeXml.Save(IO.Path.Combine(outputFolder, IO.Path.GetFileNameWithoutExtension(projectFile) + ".wxi"))

      ' Console.WriteLine("Parsed")
    Catch ex As Exception
      Console.Error.WriteLine("WixHelper: ERROR: " + ex.Message)
      Return 1
    End Try
    Return 0
  End Function
End Module
