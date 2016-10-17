Imports System.Runtime.CompilerServices
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.IO
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml.Linq

Namespace CustomXmlViewer
	Public Partial Class ucXmlRichTextBox
		Inherits RichTextBox
		#Region "Constructor"
		Public Sub New()
			InitializeComponent()

			Me.Font = New System.Drawing.Font("Consolas", 9.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CByte(0))

            AddHandler miCopyRtf.Click, AddressOf miCopyRtf_Click
            AddHandler miCopyText.Click, AddressOf miCopyText_Click
            AddHandler miSelectAll.Click, AddressOf miSelectAll_Click


        End Sub
		#End Region

		#Region "Properties"
		Private m_xml As String = ""
		Public Property Xml() As String
			Get
				Return m_xml
			End Get
			Set
				Me.Text = ""
				m_xml = value
				SetXml(m_xml)
			End Set
		End Property
		#End Region

		#Region "Private Methods"
		Private Sub SetXml(s As String)
			If [String].IsNullOrEmpty(s) Then
				Return
			End If

			Dim xdoc As XDocument = XDocument.Parse(s)

			Dim formattedText As String = xdoc.ToString().Trim()

			If [String].IsNullOrEmpty(formattedText) Then
				Return
			End If

			Dim machine As New XmlStateMachine()

			If s.StartsWith("<?") Then
				Dim xmlDeclaration As String = machine.GetXmlDeclaration(s)
				If xmlDeclaration <> [String].Empty Then
					formattedText = xmlDeclaration + Environment.NewLine & formattedText
				End If
			End If

			Dim location As Integer = 0
			Dim failCount As Integer = 0
			Dim tokenTryCount As Integer = 0
			Dim ttype As XmlTokenType = XmlTokenType.Unknown
			While location < formattedText.Length
				Dim token As String = machine.GetNextToken(formattedText, location, ttype)
				Dim color As Color = machine.GetTokenColor(ttype)
				Me.AppendText(token, color)
				location += token.Length
				tokenTryCount += 1

				' Check for ongoing failure
				If token.Length = 0 Then
					failCount += 1
				End If
				If failCount > 10 OrElse tokenTryCount > formattedText.Length Then
					Dim theRestOfIt As String = formattedText.Substring(location, formattedText.Length - location)
					'this.AppendText(Environment.NewLine + Environment.NewLine + theRestOfIt); // DEBUG
					Me.AppendText(theRestOfIt)
					Exit While
				End If
			End While
		End Sub
		#End Region

		#Region "Context Menu"
		Private Sub contextMenuStrip1_Opening(sender As Object, e As CancelEventArgs)
			If [String].IsNullOrEmpty(Me.SelectedText) Then
				miCopyText.Enabled = False
				miCopyRtf.Enabled = False
			Else
				miCopyText.Enabled = True
				miCopyRtf.Enabled = True
			End If
		End Sub

		Private Sub miCopyText_Click(sender As Object, e As EventArgs)
			Dim s As String = Me.SelectedText
			Try
				Dim doc As XDocument = XDocument.Parse(s)
				s = doc.ToString()
			Catch
			End Try
			Clipboard.SetText(s)
		End Sub

		Private Sub miCopyRtf_Click(sender As Object, e As EventArgs)
			Dim dto As New DataObject()
			dto.SetText(Me.SelectedRtf, TextDataFormat.Rtf)
			dto.SetText(Me.SelectedText, TextDataFormat.UnicodeText)
			Clipboard.Clear()
			Clipboard.SetDataObject(dto)
		End Sub

		Private Sub miSelectAll_Click(sender As Object, e As EventArgs)
			Me.SelectAll()
		End Sub
		#End Region
	End Class

#Region "Extension Methods"
    Module RichTextBoxExtensions

        <Extension()>
        Public Sub AppendText(box As RichTextBox, text As String, color As Color)
            box.SelectionStart = box.TextLength
            box.SelectionLength = 0

            box.SelectionColor = color
            box.AppendText(text)
            box.SelectionColor = box.ForeColor
        End Sub
    End Module
#End Region
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
