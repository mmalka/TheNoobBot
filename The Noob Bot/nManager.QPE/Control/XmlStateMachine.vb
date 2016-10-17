
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Text


Namespace CustomXmlViewer
	Public Class XmlStateMachine
		#Region "Public Fields"
		Public CurrentState As XmlTokenType = XmlTokenType.Unknown
		#End Region

		#Region "Private Fields"
		Private subString As String = ""
		Private token As String = String.Empty
		Private PreviousStates As New Stack(Of XmlTokenType)()
		#End Region

		#Region "Public Methods"
		Public Function GetNextToken(s As String, location As Integer, ByRef ttype As XmlTokenType) As String
			ttype = XmlTokenType.Unknown

			' skip past any whitespace (token added to it at the end of method)
			Dim whitespace As String = GetWhitespace(s, location)
			If Not [String].IsNullOrEmpty(whitespace) Then
				location += whitespace.Length
			End If

			subString = s.Substring(location, s.Length - location)
			token = String.Empty

			If CurrentState = XmlTokenType.CDataStart Then
				' check for empty CDATA
				If subString.StartsWith("]]>") Then
					CurrentState = XmlTokenType.CDataEnd
					token = "]]>"
				Else
					CurrentState = XmlTokenType.CDataValue
					Dim n As Integer = subString.IndexOf("]]>")
					token = subString.Substring(0, n)
				End If
			ElseIf CurrentState = XmlTokenType.DocTypeStart Then
				CurrentState = XmlTokenType.DocTypeName
				token = "DOCTYPE"
			ElseIf CurrentState = XmlTokenType.DocTypeName Then
				CurrentState = XmlTokenType.DocTypeDeclaration
				Dim n As Integer = subString.IndexOf("[")
				token = subString.Substring(0, n)
			ElseIf CurrentState = XmlTokenType.DocTypeDeclaration Then
				CurrentState = XmlTokenType.DocTypeDefStart
				token = "["
			ElseIf CurrentState = XmlTokenType.DocTypeDefStart Then
				If subString.StartsWith("]>") Then
					CurrentState = XmlTokenType.DocTypeDefEnd
					token = "]>"
				Else
					CurrentState = XmlTokenType.DocTypeDefValue
					Dim n As Integer = subString.IndexOf("]>")
					token = subString.Substring(0, n)
				End If
			ElseIf CurrentState = XmlTokenType.DocTypeDefValue Then
				CurrentState = XmlTokenType.DocTypeDefEnd
				token = "]>"
			ElseIf CurrentState = XmlTokenType.DoubleQuotationMarkStart Then
				' check for empty attribute value
				If subString(0) = """"C Then
					CurrentState = XmlTokenType.DoubleQuotationMarkEnd
					token = """"
				Else
					CurrentState = XmlTokenType.AttributeValue
					Dim n As Integer = subString.IndexOf("""")
					token = subString.Substring(0, n)
				End If
			ElseIf CurrentState = XmlTokenType.SingleQuotationMarkStart Then
				' check for empty attribute value
				If subString(0) = "'"C Then
					CurrentState = XmlTokenType.SingleQuotationMarkEnd
					token = "'"
				Else
					CurrentState = XmlTokenType.AttributeValue
					Dim n As Integer = subString.IndexOf("'")
					token = subString.Substring(0, n)
				End If
			ElseIf CurrentState = XmlTokenType.CommentStart Then
				' check for empty comment
				If subString.StartsWith("-->") Then
					CurrentState = XmlTokenType.CommentEnd
					token = "-->"
				Else
					CurrentState = XmlTokenType.CommentValue
					token = ReadCommentValue(subString, location)
				End If
			ElseIf CurrentState = XmlTokenType.NodeStart Then
				CurrentState = XmlTokenType.NodeName
				token = ReadNodeName(subString, location)
			ElseIf CurrentState = XmlTokenType.XmlDeclarationStart Then
				CurrentState = XmlTokenType.NodeName
				token = ReadNodeName(subString, location)
			ElseIf CurrentState = XmlTokenType.NodeName Then
				If subString(0) <> "/"C AndAlso subString(0) <> ">"C Then
					CurrentState = XmlTokenType.AttributeName
					token = ReadAttributeName(subString, location)
				Else
					HandleReservedXmlToken()
				End If
			ElseIf CurrentState = XmlTokenType.NodeEndValueStart Then
				If subString(0) = "<"C Then
					HandleReservedXmlToken()
				Else
					CurrentState = XmlTokenType.NodeValue
					token = ReadNodeValue(subString, location)
				End If
			ElseIf CurrentState = XmlTokenType.DoubleQuotationMarkEnd Then
				HandleAttributeEnd(location)
			ElseIf CurrentState = XmlTokenType.SingleQuotationMarkEnd Then
				HandleAttributeEnd(location)
			Else
				HandleReservedXmlToken()
			End If

			If token <> String.Empty Then
				ttype = CurrentState
				Return whitespace & token
			End If

			Return String.Empty

		End Function

		Public Function GetTokenColor(ttype As XmlTokenType) As Color
			Dim brown As Color = Color.FromArgb(238, 149, 68)

			Select Case ttype
				Case XmlTokenType.NodeValue, XmlTokenType.EqualSignStart, XmlTokenType.EqualSignEnd, XmlTokenType.DoubleQuotationMarkStart, XmlTokenType.DoubleQuotationMarkEnd, XmlTokenType.SingleQuotationMarkStart, _
					XmlTokenType.SingleQuotationMarkEnd
					Return Color.Black

				Case XmlTokenType.XmlDeclarationStart, XmlTokenType.XmlDeclarationEnd, XmlTokenType.NodeStart, XmlTokenType.NodeEnd, XmlTokenType.NodeEndValueStart, XmlTokenType.CDataStart, _
					XmlTokenType.CDataEnd, XmlTokenType.CommentStart, XmlTokenType.CommentEnd, XmlTokenType.AttributeValue, XmlTokenType.DocTypeStart, XmlTokenType.DocTypeEnd, _
					XmlTokenType.DocTypeDefStart, XmlTokenType.DocTypeDefEnd
					Return Color.Blue

				Case XmlTokenType.CDataValue, XmlTokenType.DocTypeDefValue
					Return Color.Gray

				Case XmlTokenType.CommentValue
					Return Color.Green

				Case XmlTokenType.DocTypeName, XmlTokenType.NodeName
                    Return Color.Blue

                Case XmlTokenType.AttributeName, XmlTokenType.DocTypeDeclaration
					Return Color.Red
				Case Else

					Return Color.Orange
			End Select
		End Function

		Public Function GetXmlDeclaration(s As String) As String
			Dim start As Integer = s.IndexOf("<?")
			Dim [end] As Integer = s.IndexOf("?>")
			If start > -1 AndAlso [end] > start Then
				Return s.Substring(start, [end] - start + 2)
			Else
				Return String.Empty
			End If
		End Function
		#End Region

		#Region "Private Methods"
		Private Sub HandleAttributeEnd(location As Integer)
			If subString.StartsWith(">") Then
				HandleReservedXmlToken()
			ElseIf subString.StartsWith("/>") Then
				HandleReservedXmlToken()
			ElseIf subString.StartsWith("?>") Then
				HandleReservedXmlToken()
			Else
				CurrentState = XmlTokenType.AttributeName
				token = ReadAttributeName(subString, location)
			End If
		End Sub

		Private Sub HandleReservedXmlToken()
			' check if state changer
			' <, >, =, </, />, <![CDATA[, <!--, -->

			If subString.StartsWith("<![CDATA[") Then
				CurrentState = XmlTokenType.CDataStart
				token = "<![CDATA["
			ElseIf subString.StartsWith("<!DOCTYPE") Then
				CurrentState = XmlTokenType.DocTypeStart
				token = "<!"
			ElseIf subString.StartsWith("</") Then
				CurrentState = XmlTokenType.NodeStart
				token = "</"
			ElseIf subString.StartsWith("<!--") Then
				CurrentState = XmlTokenType.CommentStart
				token = "<!--"
			ElseIf subString.StartsWith("<?") Then
				CurrentState = XmlTokenType.XmlDeclarationStart
				token = "<?"
			ElseIf subString.StartsWith("<") Then
				CurrentState = XmlTokenType.NodeStart
				token = "<"
			ElseIf subString.StartsWith("=") Then
				CurrentState = XmlTokenType.EqualSignStart
				If CurrentState = XmlTokenType.AttributeValue Then
					CurrentState = XmlTokenType.EqualSignEnd
				End If
				token = "="
			ElseIf subString.StartsWith("?>") Then
				CurrentState = XmlTokenType.XmlDeclarationEnd
				token = "?>"
			ElseIf subString.StartsWith(">") Then
				CurrentState = XmlTokenType.NodeEndValueStart
				token = ">"
			ElseIf subString.StartsWith("-->") Then
				CurrentState = XmlTokenType.CommentEnd
				token = "-->"
			ElseIf subString.StartsWith("]>") Then
				CurrentState = XmlTokenType.DocTypeEnd
				token = "]>"
			ElseIf subString.StartsWith("]]>") Then
				CurrentState = XmlTokenType.CDataEnd
				token = "]]>"
			ElseIf subString.StartsWith("/>") Then
				CurrentState = XmlTokenType.NodeEnd
				token = "/>"
			ElseIf subString.StartsWith("""") Then
				If CurrentState = XmlTokenType.AttributeValue Then
					CurrentState = XmlTokenType.DoubleQuotationMarkEnd
				Else
					CurrentState = XmlTokenType.DoubleQuotationMarkStart
				End If
				token = """"
			ElseIf subString.StartsWith("'") Then
				CurrentState = XmlTokenType.SingleQuotationMarkStart
				If CurrentState = XmlTokenType.AttributeValue Then
					CurrentState = XmlTokenType.SingleQuotationMarkEnd
				End If
				token = "'"
			End If
		End Sub

		Private Function GetAttributeTokens(s As String) As List(Of String)
			Dim list As New List(Of String)()
			Dim arr As String() = s.Split(" "C)
			For i As Integer = 0 To arr.Length - 1
				arr(i) = arr(i).Trim()
				If arr(i).Length > 0 Then
					list.Add(arr(i))
				End If
			Next
			Return list
		End Function

		Private Function ReadNodeName(s As String, location As Integer) As String
			Dim nodeName As String = ""

			For i As Integer = 0 To s.Length - 1
				If s(i) = "/"C OrElse s(i) = " "C OrElse s(i) = ">"C Then
					Return nodeName
				Else
					nodeName += s(i).ToString()
				End If
			Next

			Return nodeName
		End Function

		Private Function ReadAttributeName(s As String, location As Integer) As String
			Dim attName As String = ""

			Dim n As Integer = s.IndexOf("="C)
			If n <> -1 Then
				attName = s.Substring(0, n)
			End If

			Return attName
		End Function

		Private Function ReadNodeValue(s As String, location As Integer) As String
			Dim nodeValue As String = ""

			Dim n As Integer = s.IndexOf("<"C)
			If n <> -1 Then
				nodeValue = s.Substring(0, n)
			End If

			Return nodeValue
		End Function

		Private Function ReadCommentValue(s As String, location As Integer) As String
			Dim commentValue As String = ""

			Dim n As Integer = s.IndexOf("-->")
			If n <> -1 Then
				commentValue = s.Substring(0, n)
			End If

			Return commentValue
		End Function



		Private Function GetWhitespace(s As String, location As Integer) As String
			Dim foundWhitespace As Boolean = False
			Dim sb As New StringBuilder()
			Dim i As Integer = 0
			While (location + i) < s.Length
				Dim c As Char = s(location + i)
				If [Char].IsWhiteSpace(c) Then
					foundWhitespace = True
					sb.Append(c)
				Else
					Exit While
				End If
				i += 1
			End While
			If foundWhitespace Then
				Return sb.ToString()
			End If
			Return [String].Empty
		End Function
		#End Region
	End Class

	Public Enum XmlTokenType
		Whitespace
		XmlDeclarationStart
		XmlDeclarationEnd
		NodeStart
		NodeEnd
		NodeEndValueStart
		NodeName
		NodeValue
		AttributeName
		AttributeValue
		EqualSignStart
		EqualSignEnd
		CommentStart
		CommentValue
		CommentEnd
		CDataStart
		CDataValue
		CDataEnd
		DoubleQuotationMarkStart
		DoubleQuotationMarkEnd
		SingleQuotationMarkStart
		SingleQuotationMarkEnd
		DocTypeStart
		DocTypeName
		DocTypeDeclaration
		DocTypeDefStart
		DocTypeDefValue
		DocTypeDefEnd
		DocTypeEnd
		DocumentEnd
		Unknown
	End Enum
End Namespace

'=======================================================
'Service provided by Telerik (www.telerik.com)
'Conversion powered by NRefactory.
'Twitter: @telerik
'Facebook: facebook.com/telerik
'=======================================================
