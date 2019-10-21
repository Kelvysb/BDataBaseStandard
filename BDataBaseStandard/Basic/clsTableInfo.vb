
'Copyright 2018 Kelvys B. Pantaleão

'This file is part of BDataBase

'BDataBase Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program.  If Not, see <http://www.gnu.org/licenses/>.


'Este arquivo é parte Do programa BDataBase

'BDataBase é um software livre; você pode redistribuí-lo e/ou 
'modificá-lo dentro dos termos da Licença Pública Geral GNU como 
'publicada pela Fundação Do Software Livre (FSF); na versão 3 da 
'Licença, ou(a seu critério) qualquer versão posterior.

'Este programa é distribuído na esperança de que possa ser  útil, 
'mas SEM NENHUMA GARANTIA; sem uma garantia implícita de ADEQUAÇÃO
'a qualquer MERCADO ou APLICAÇÃO EM PARTICULAR. Veja a
'Licença Pública Geral GNU para maiores detalhes.

'Você deve ter recebido uma cópia da Licença Pública Geral GNU junto
'com este programa, Se não, veja <http://www.gnu.org/licenses/>.

'GitHub: https://github.com/Kelvysb/BDataBase

Imports Newtonsoft

Public Enum enm_ColumnType
    Text
    Number
    Int
    Bool
    DateTime
    Xml
    File
    Other
End Enum

Public Class clsTableInfo
#Region "Declarations"
    Private strTableName As String
    Private objColumns As List(Of clsColumnInfo)
    Private objIndexes As List(Of clsIndexInfo)
#End Region

#Region "Constructor"
    Public Sub New()
        Try
            strTableName = ""
            objColumns = New List(Of clsColumnInfo)
            objIndexes = New List(Of clsIndexInfo)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub New(p_xmlInput As XDocument)
        Try
            Call sbfromXml(p_xmlInput)
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Functions"
    Public Function toXml() As XDocument

        Dim xmlReturn As XDocument

        Try

            xmlReturn = New XDocument(<TableInfo>
                                          <Name><%= strTableName.Trim %></Name>
                                          <Columns></Columns>
                                          <Indexes></Indexes>
                                      </TableInfo>)

            For Each Column As clsColumnInfo In objColumns
                xmlReturn.<TableInfo>.<Columns>.First.Add(Column.toXml.Elements)
            Next

            For Each Index As clsIndexInfo In objIndexes
                xmlReturn.<TableInfo>.<Indexes>.First.Add(Index.toXml.Elements)
            Next

            Return xmlReturn

        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Sub sbfromXml(p_xmlInput As XDocument)
        Try


            strTableName = p_xmlInput.<TableInfo>.<Name>.Value.Trim

            objColumns = New List(Of clsColumnInfo)
            For I = 0 To p_xmlInput.<TableInfo>.<Columns>.<ColumnInfo>.Count - 1
                objColumns.Add(New clsColumnInfo(New XDocument(p_xmlInput.<TableInfo>.<Columns>.<ColumnInfo>(I))))
            Next

            objIndexes = New List(Of clsIndexInfo)
            For I = 0 To p_xmlInput.<TableInfo>.<Indexes>.<IndexInfo>.Count - 1
                objIndexes.Add(New clsIndexInfo(New XDocument(p_xmlInput.<TableInfo>.<Indexes>.<IndexInfo>(I))))
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function clone() As clsTableInfo
        Try
            Return New clsTableInfo(toXml)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function serialize() As String
        Try
            Return Json.JsonConvert.SerializeObject(Me)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function serialize(p_objInput As clsTableInfo) As String
        Try
            Return Json.JsonConvert.SerializeObject(p_objInput)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function deserialize(p_strJson As String) As clsTableInfo
        Try
            Return Json.JsonConvert.DeserializeObject(Of clsTableInfo)(p_strJson)
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "Properties"
    <Json.JsonProperty("NAME")>
    Public Property Name() As String
        Get
            Return strTableName
        End Get
        Set(ByVal value As String)
            strTableName = value
        End Set
    End Property
    <Json.JsonProperty("COLUMNS")>
    Public Property Columns() As List(Of clsColumnInfo)
        Get
            Return objColumns
        End Get
        Set(ByVal value As List(Of clsColumnInfo))
            objColumns = value
        End Set
    End Property
    <Json.JsonProperty("INDEXES")>
    Public Property Indexes() As List(Of clsIndexInfo)
        Get
            Return objIndexes
        End Get
        Set(ByVal value As List(Of clsIndexInfo))
            objIndexes = value
        End Set
    End Property
#End Region
End Class

Public Class clsColumnInfo
#Region "Declarations"
    Private intColumnIndex As String
    Private strColumnName As String
    Private enmColumnType As enm_ColumnType
    Private intColumnSize As Integer
    Private blnPrimaryKey As Boolean
    Private strDescription As String
#End Region

#Region "Constructor"
    Public Sub New()
        Try
            intColumnIndex = 0
            strColumnName = ""
            enmColumnType = enm_ColumnType.Text
            intColumnSize = 0
            blnPrimaryKey = False
            strDescription = ""
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub New(p_xmlInput As XDocument)
        Try
            Call sbfromXml(p_xmlInput)
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Functions"
    Public Function toXml() As XDocument

        Dim xmlReturn As XDocument

        Try

            xmlReturn = New XDocument(<ColumnInfo>
                                          <Index><%= intColumnIndex %></Index>
                                          <Name><%= strColumnName.Trim %></Name>
                                          <Type><%= enmColumnType.ToString %></Type>
                                          <Size><%= intColumnSize %></Size>
                                          <PrimaryKey><%= blnPrimaryKey.ToString %></PrimaryKey>
                                          <Description><%= strDescription %></Description>
                                      </ColumnInfo>)

            Return xmlReturn

        Catch ex As Exception
            Throw
        End Try
    End Function

    Protected Sub sbFromXml(p_xmlIntput As XDocument)

        Try

            Integer.TryParse(p_xmlIntput.<ColumnInfo>.<Index>.Value, intColumnIndex)
            strColumnName = p_xmlIntput.<ColumnInfo>.<Name>.Value.Trim
            [Enum].TryParse(Of enm_ColumnType)(p_xmlIntput.<ColumnInfo>.<Type>.Value, True, enmColumnType)
            Integer.TryParse(p_xmlIntput.<ColumnInfo>.<Size>.Value, intColumnSize)
            Boolean.TryParse(p_xmlIntput.<ColumnInfo>.<PrimaryKey>.Value, blnPrimaryKey)
            strDescription = p_xmlIntput.<ColumnInfo>.<Description>.Value.Trim

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function clone() As clsColumnInfo
        Try
            Return New clsColumnInfo(toXml)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function serialize() As String
        Try
            Return Json.JsonConvert.SerializeObject(Me)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function serialize(p_objInput As clsColumnInfo) As String
        Try
            Return Json.JsonConvert.SerializeObject(p_objInput)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function deserialize(p_strJson As String) As clsColumnInfo
        Try
            Return Json.JsonConvert.DeserializeObject(Of clsColumnInfo)(p_strJson)
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "Properties"
    <Json.JsonProperty("INDEX")>
    Public Property Index() As String
        Get
            Return intColumnIndex
        End Get
        Set(ByVal value As String)
            intColumnIndex = value
        End Set
    End Property
    <Json.JsonProperty("NAME")>
    Public Property Name() As String
        Get
            Return strColumnName
        End Get
        Set(ByVal value As String)
            strColumnName = value
        End Set
    End Property
    <Json.JsonProperty("TYPE")>
    Public Property Type() As enm_ColumnType
        Get
            Return enmColumnType
        End Get
        Set(ByVal value As enm_ColumnType)
            enmColumnType = value
        End Set
    End Property
    <Json.JsonProperty("SIZE")>
    Public Property Size() As Integer
        Get
            Return intColumnSize
        End Get
        Set(ByVal value As Integer)
            intColumnSize = value
        End Set
    End Property
    <Json.JsonProperty("PRIMARYKEY")>
    Public Property PrimaryKey() As Boolean
        Get
            Return blnPrimaryKey
        End Get
        Set(ByVal value As Boolean)
            blnPrimaryKey = value
        End Set
    End Property
    <Json.JsonProperty("DESCRIPTION")>
    Public Property Description() As String
        Get
            Return strDescription
        End Get
        Set(ByVal value As String)
            strDescription = value
        End Set
    End Property
#End Region
End Class

Public Class clsIndexInfo
#Region "Declarations"
    Private strIndexName As String
    Private objColumns As List(Of clsColumnInfo)
#End Region

#Region "Constructor"
    Public Sub New()
        Try
            strIndexName = ""
            objColumns = New List(Of clsColumnInfo)
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub New(p_xmlInput As XDocument)
        Try
            Call sbfromXml(p_xmlInput)
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Functions"
    Public Function toXml() As XDocument

        Dim xmlReturn As XDocument

        Try

            xmlReturn = New XDocument(<IndexInfo>
                                          <Name><%= strIndexName.Trim %></Name>
                                          <Columns></Columns>
                                      </IndexInfo>)

            For Each Column As clsColumnInfo In objColumns
                xmlReturn.<IndexInfo>.<Columns>.First.Add(Column.toXml.elements)
            Next

            Return xmlReturn

        Catch ex As Exception
            Throw
        End Try
    End Function
    Protected Sub sbfromXml(p_xmlInput As XDocument)
        Try

            strIndexName = p_xmlInput.<IndexInfo>.<Name>.Value.Trim

            objColumns = New List(Of clsColumnInfo)
            For I = 0 To p_xmlInput.<IndexInfo>.<Columns>.<ColumnInfo>.Count - 1
                objColumns.Add(New clsColumnInfo(New XDocument(p_xmlInput.<IndexInfo>.<Columns>.<ColumnInfo>(I))))
            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Function clone() As clsIndexInfo
        Try
            Return New clsIndexInfo(toXml)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Function serialize() As String
        Try
            Return Json.JsonConvert.SerializeObject(Me)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function serialize(p_objInput As clsIndexInfo) As String
        Try
            Return Json.JsonConvert.SerializeObject(p_objInput)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function deserialize(p_strJson As String) As clsIndexInfo
        Try
            Return Json.JsonConvert.DeserializeObject(Of clsIndexInfo)(p_strJson)
        Catch ex As Exception
            Throw
        End Try
    End Function
#End Region

#Region "Properties"
    <Json.JsonProperty("NAME")>
    Public Property Name() As String
        Get
            Return strIndexName
        End Get
        Set(ByVal value As String)
            strIndexName = value
        End Set
    End Property
    <Json.JsonProperty("COLUMNS")>
    Public Property Columns() As List(Of clsColumnInfo)
        Get
            Return objColumns
        End Get
        Set(ByVal value As List(Of clsColumnInfo))
            objColumns = value
        End Set
    End Property
#End Region
End Class