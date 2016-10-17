
Namespace CustomXmlViewer
	Partial Class ucXmlRichTextBox
		''' <summary> 
		''' Required designer variable.
		''' </summary>
		Private components As System.ComponentModel.IContainer = Nothing

		''' <summary> 
		''' Clean up any resources being used.
		''' </summary>
		''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		Protected Overrides Sub Dispose(disposing As Boolean)
			If disposing AndAlso (components IsNot Nothing) Then
				components.Dispose()
			End If
			MyBase.Dispose(disposing)
		End Sub

		#Region "Component Designer generated code"

		''' <summary> 
		''' Required method for Designer support - do not modify 
		''' the contents of this method with the code editor.
		''' </summary>
		Private Sub InitializeComponent()
			Me.components = New System.ComponentModel.Container()
			Me.contextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
			Me.miCopyText = New System.Windows.Forms.ToolStripMenuItem()
			Me.miCopyRtf = New System.Windows.Forms.ToolStripMenuItem()
			Me.toolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
			Me.miSelectAll = New System.Windows.Forms.ToolStripMenuItem()
			Me.contextMenuStrip1.SuspendLayout()
			Me.SuspendLayout()
			' 
			' contextMenuStrip1
			' 
			Me.contextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.miCopyText, Me.miCopyRtf, Me.toolStripSeparator1, Me.miSelectAll})
			Me.contextMenuStrip1.Name = "contextMenuStrip1"
			Me.contextMenuStrip1.Size = New System.Drawing.Size(154, 76)

            ' 
            ' miCopyText
            ' 
            Me.miCopyText.Name = "miCopyText"
			Me.miCopyText.Size = New System.Drawing.Size(153, 22)
			Me.miCopyText.Text = "Copy Text"
			' 
			' miCopyRtf
			' 
			Me.miCopyRtf.Name = "miCopyRtf"
			Me.miCopyRtf.Size = New System.Drawing.Size(153, 22)
			Me.miCopyRtf.Text = "Copy Rich Text"
			' 
			' toolStripSeparator1
			' 
			Me.toolStripSeparator1.Name = "toolStripSeparator1"
			Me.toolStripSeparator1.Size = New System.Drawing.Size(150, 6)
			' 
			' miSelectAll
			' 
			Me.miSelectAll.Name = "miSelectAll"
			Me.miSelectAll.Size = New System.Drawing.Size(153, 22)
			Me.miSelectAll.Text = "Select All"
			' 
			' ucXmlRichTextBox
			' 
			Me.ContextMenuStrip = Me.contextMenuStrip1
			Me.contextMenuStrip1.ResumeLayout(False)
			Me.ResumeLayout(False)

		End Sub

		#End Region

		Private contextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
		Private miCopyText As System.Windows.Forms.ToolStripMenuItem
		Private miCopyRtf As System.Windows.Forms.ToolStripMenuItem
		Private toolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
		Private miSelectAll As System.Windows.Forms.ToolStripMenuItem

	End Class
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
