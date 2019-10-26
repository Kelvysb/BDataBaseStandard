
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

Imports System.Data
Imports System.Threading
Imports Npgsql

Friend Class DataBase_Postgre
    Inherits absDataBase
    Implements IDataBase

#Region "Definitions"
    Private objSqlConnection As NpgsqlConnection
    Private objSqlCommand As NpgsqlCommand
    Private objSqlDataAdapter As NpgsqlDataAdapter
    Private objSqlTransact As NpgsqlTransaction
#End Region

#Region "Constructor"
    Friend Sub New(ByVal p_strServer As String, ByVal p_strDataBase As String, ByVal p_strUser As String,
                    ByVal p_strPassword As String, p_intId As Integer, p_intConnectionTimeout As Integer)
        Call MyBase.New(p_strServer, p_strDataBase, p_strUser, p_strPassword, p_intId, p_intConnectionTimeout)
    End Sub

    Friend Sub New(ByVal p_strConnectionString As String)
        Call MyBase.New(p_strConnectionString)
    End Sub

    Friend Sub New(ByVal p_strConnectionString As String, p_intId As Integer)
        Call MyBase.New(p_strConnectionString, p_intId)
    End Sub
#End Region

#Region "Functions and Subroutines"
    Public Overrides Sub sbOpen() Implements IDataBase.sbOpen

        Dim strConnection As String
        Dim strAuxPort As String
        Dim strAuxServer As String

        Try


            If objSqlConnection Is Nothing = False Then
                If objSqlConnection.State = ConnectionState.Open Then
                    Try
                        objSqlConnection.Close()
                        objSqlConnection = Nothing
                    Catch ex As Exception

                    End Try
                End If
            End If


            If strConnectionString.Equals("") Then
                strAuxPort = ""
                strAuxServer = ""
                If strServer.Contains(",") = True Then
                    strAuxServer = strServer.Split(",")(0)
                    strAuxPort = strServer.Split(",")(1)
                End If

                If strServer.Contains(":") = True And strAuxServer.Trim = "" Then
                    strAuxServer = strServer.Split(":")(0)
                    strAuxPort = strServer.Split(":")(1)
                End If

                If strAuxServer.Trim = "" Then
                    strConnection = "Server=" & strServer & ";Database=" & strDataBase & ";User Id=" &
                         strUser & ";Password=" & strPassword & ";Timeout=" & intConnectionTimeout
                Else
                    strConnection = "Server=" & strAuxServer & ";Port=" & strAuxPort & ";Database=" & strDataBase & ";User Id=" &
                         strUser & ";Password=" & strPassword & ";Timeout=" & intConnectionTimeout
                End If
            Else
                strConnection = strConnectionString
            End If


            'Create Connection
            objSqlConnection = New NpgsqlConnection(strConnection)
            objSqlConnection.Open()

        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub



    Public Overrides Sub sbClose() Implements IDataBase.sbClose
        Try
            If objSqlConnection Is Nothing = False Then
                If objSqlConnection.State = ConnectionState.Open Then
                    objSqlConnection.Close()
                    objSqlConnection = Nothing
                End If
            End If
        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub
    Public Overrides Sub sbBegin() Implements IDataBase.sbBegin
        Try

            If objSqlConnection Is Nothing Then
                Call sbOpen()
            End If

            If objSqlConnection.State = ConnectionState.Closed Or objSqlConnection.State = ConnectionState.Broken Then
                Call sbOpen()
            End If

            If objSqlConnection.State <> ConnectionState.Open Then
                objSqlConnection.Open()
            End If

            If Not blnBeginTransaction Then
                objSqlTransact = objSqlConnection.BeginTransaction()
                blnBeginTransaction = True
            End If

        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub
    Public Overrides Sub sbCommit() Implements IDataBase.sbCommit
        Try

            If objSqlConnection Is Nothing Then
                Call sbOpen()
            End If

            If objSqlConnection.State = ConnectionState.Closed Or objSqlConnection.State = ConnectionState.Broken Then
                Call sbOpen()
            End If

            If objSqlConnection.State <> ConnectionState.Open Then
                objSqlConnection.Open()
            End If

            If blnBeginTransaction = True Then
                objSqlTransact.Commit()
                blnBeginTransaction = False
            End If

        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub
    Public Overrides Sub sbRollBack() Implements IDataBase.sbRollBack
        Try
            If objSqlConnection Is Nothing Then
                Call sbOpen()
            End If

            If objSqlConnection.State = ConnectionState.Closed Or objSqlConnection.State = ConnectionState.Broken Then
                Call sbOpen()
            End If

            If objSqlConnection.State <> ConnectionState.Open Then
                objSqlConnection.Open()
            End If

            If blnBeginTransaction = True Then
                objSqlTransact.Rollback()
                blnBeginTransaction = False
            End If

        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub

    Public Overrides Sub sbExecute(p_strCommand As String, ByVal p_intTimeout As Integer, p_objParameters As List(Of clsDataBaseParametes)) Implements IDataBase.sbExecute

        Dim intTries As Integer
        Dim objAuxException As DataBaseException
        Dim blnReconnect As Boolean
        Dim intReturn As Integer

        Try

            intTries = 0
            blnReconnect = False

            Do

                If objSqlConnection Is Nothing Then
                    Call sbOpen()
                End If

                If objSqlConnection.State = ConnectionState.Closed Or objSqlConnection.State = ConnectionState.Broken Then
                    Call sbOpen()
                End If

                If objSqlConnection.State <> ConnectionState.Open Then
                    objSqlConnection.Open()
                End If

                objSqlCommand = New NpgsqlCommand(p_strCommand, objSqlConnection)

                objSqlCommand.CommandTimeout = p_intTimeout

                If blnBeginTransaction = False Then
                    objSqlTransact = objSqlConnection.BeginTransaction()
                    blnBeginTransaction = True
                End If

                objSqlCommand.Transaction = objSqlTransact

                If p_objParameters.Count > 0 Then
                    For Each Parameter As clsDataBaseParametes In p_objParameters
                        objSqlCommand.Parameters.AddWithValue(Parameter.Key, Parameter.Value)
                    Next
                End If

                Try

                    intTries = intTries + 1

                    intReturn = objSqlCommand.ExecuteNonQuery()

                    If intReturn = 0 Then
                        Throw New DataBaseException(DataBaseException.enmDataBaseExeptionCode.NotExists, "Not Exists")
                    End If

                    Exit Do

                Catch exBd As DataBaseException

                    If exBd.Code = DataBaseException.enmDataBaseExeptionCode.Erro Or exBd.Code = DataBaseException.enmDataBaseExeptionCode.ConnectionClosed Then
                        If intTries <= CNS_TRIES Then
                            Thread.Sleep(CNS_TRIESDELAY)
                        Else
                            Throw exBd
                        End If
                    Else
                        Throw exBd
                    End If

                Catch ex As NpgsqlException

                    objAuxException = New DataBaseException(ex)
                    If objAuxException.Code = DataBaseException.enmDataBaseExeptionCode.Erro Or objAuxException.Code = DataBaseException.enmDataBaseExeptionCode.ConnectionClosed Then
                        If intTries <= CNS_TRIES Then
                            Thread.Sleep(CNS_TRIESDELAY)
                        Else
                            Throw objAuxException
                        End If
                    Else
                        Throw objAuxException
                    End If

                Catch ex As Exception

                    If intTries <= CNS_TRIES Then
                        Thread.Sleep(CNS_TRIESDELAY)
                        blnReconnect = True
                    Else
                        Throw ex
                    End If

                End Try

            Loop

        Catch ex As DataBaseException
            Throw ex
        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub
    Public Overrides Sub sbExecute(p_strCommand As String, p_intTimeout As Integer, p_objParameters As Dictionary(Of String, Object))
        sbExecute(p_strCommand, p_intTimeout, p_objParameters.Select(Function(item) New clsDataBaseParametes(item.Key, item.Value)).ToList)
    End Sub

    Public Overrides Function fnExecute(ByVal p_strCommand As String, ByVal p_intTimeout As Integer, p_objParameters As List(Of clsDataBaseParametes)) As DataSet Implements IDataBase.fnExecute

        Dim objDataSet As New DataSet
        Dim intTries As Integer
        Dim objAuxException As DataBaseException
        Dim blnReconnect As Boolean

        Try

            intTries = 0
            blnReconnect = False

            Do

                If objSqlConnection Is Nothing Then
                    Call sbOpen()
                End If

                If objSqlConnection.State = ConnectionState.Closed Or objSqlConnection.State = ConnectionState.Broken Then
                    Call sbOpen()
                End If

                If objSqlConnection.State <> ConnectionState.Open Then
                    objSqlConnection.Open()
                End If

                objSqlCommand = New NpgsqlCommand(p_strCommand, objSqlConnection)

                objSqlCommand.CommandTimeout = p_intTimeout

                If blnBeginTransaction = True Then
                    objSqlCommand.Transaction = objSqlTransact
                End If

                If p_objParameters.Count > 0 Then
                    For Each Parameter As clsDataBaseParametes In p_objParameters
                        objSqlCommand.Parameters.AddWithValue(Parameter.Key, Parameter.Value)
                    Next
                End If

                Try

                    intTries = intTries + 1

                    objSqlDataAdapter = New NpgsqlDataAdapter(objSqlCommand)
                    objSqlDataAdapter.Fill(objDataSet)

                    If objDataSet.Tables.Count > 0 AndAlso objDataSet.Tables(0).Rows.Count <= 0 Then
                        Throw New DataBaseException(DataBaseException.enmDataBaseExeptionCode.NotExists, "Not exists")
                    End If

                    Exit Do

                Catch exBd As DataBaseException

                    If exBd.Code = DataBaseException.enmDataBaseExeptionCode.Erro Or exBd.Code = DataBaseException.enmDataBaseExeptionCode.ConnectionClosed Then
                        If intTries <= CNS_TRIES Then
                            Thread.Sleep(CNS_TRIESDELAY)
                        Else
                            Throw exBd
                        End If
                    Else
                        Throw exBd
                    End If

                Catch ex As NpgsqlException

                    objAuxException = New DataBaseException(ex)
                    If objAuxException.Code = DataBaseException.enmDataBaseExeptionCode.Erro Or objAuxException.Code = DataBaseException.enmDataBaseExeptionCode.ConnectionClosed Then
                        If intTries <= CNS_TRIES Then
                            Thread.Sleep(CNS_TRIESDELAY)
                        Else
                            Throw objAuxException
                        End If
                    Else
                        Throw objAuxException
                    End If

                Catch ex As Exception

                    If intTries <= CNS_TRIES Then
                        Thread.Sleep(CNS_TRIESDELAY)
                        blnReconnect = True
                    Else
                        Throw ex
                    End If

                End Try

            Loop

            Return objDataSet

        Catch ex As DataBaseException
            Throw ex
        Catch ex As NpgsqlException
            Throw New DataBaseException(ex)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Function

    Public Overrides Function fnExecute(p_strCommand As String, p_intTimeout As Integer, p_objParameters As Dictionary(Of String, Object)) As DataSet
        Return fnExecute(p_strCommand, p_intTimeout, p_objParameters.Select(Function(item) New clsDataBaseParametes(item.Key, item.Value)).ToList)
    End Function

    Public Overrides Function fnGetConfiguration() As clsConfiguration

        Dim objReturn As clsConfiguration
        Try

            objReturn = MyBase.fnGetConfiguration()

            objReturn.Type = BDataBaseStandard.DataBase.enmDataBaseType.MySql

            Return objReturn

        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Function

    Public Overrides Function fnGetTableInfo(p_strTable As String) As clsTableInfo Implements IDataBase.fnGetTableInfo

        Dim objReturn As clsTableInfo
        Dim strCommand As String
        Dim objAuxDataSet As DataSet

        Try

            objReturn = New clsTableInfo With {.Name = p_strTable}

            'Table info with columns
            strCommand = "SELECT" & vbNewLine
            strCommand = strCommand & "c.column_name as name, " & vbNewLine
            strCommand = strCommand & "c.data_type as type, " & vbNewLine
            strCommand = strCommand & "coalesce(character_octet_length, 0) AS size, " & vbNewLine
            strCommand = strCommand & "case when coalesce(kc.column_name, '') = '' then 'NO' else 'YES' end as PrimaryKey" & vbNewLine
            strCommand = strCommand & "FROM" & vbNewLine
            strCommand = strCommand & "information_schema.columns c  " & vbNewLine
            strCommand = strCommand & "LEFT JOIN information_schema.table_constraints tc ON " & vbNewLine
            strCommand = strCommand & "c.table_schema = tc.constraint_schema " & vbNewLine
            strCommand = strCommand & "AND tc.table_name = c.table_name" & vbNewLine
            strCommand = strCommand & "LEFT join information_schema.key_column_usage kc on " & vbNewLine
            strCommand = strCommand & "kc.table_name = tc.table_name " & vbNewLine
            strCommand = strCommand & "and kc.table_schema = tc.table_schema " & vbNewLine
            strCommand = strCommand & "and kc.constraint_name = tc.constraint_name " & vbNewLine
            strCommand = strCommand & "and kc.column_name = c.column_name" & vbNewLine
            strCommand = strCommand & "where constraint_type = 'PRIMARY KEY' and tc.table_name = '" & p_strTable & "'"
            objAuxDataSet = fnExecute(strCommand)

            objReturn.Columns = (From Register As DataRow In objAuxDataSet.Tables(0).Rows
                                 Select New clsColumnInfo() With {.Index = objAuxDataSet.Tables(0).Rows.IndexOf(Register),
                                                                  .Name = Register("name"),
                                                                  .Type = fnSelectType(Register("type")),
                                                                  .Size = CInt(Register("size")),
                                                                  .PrimaryKey = If(Register("PrimaryKey").ToString.Trim.ToUpper = "YES", True, False)}).ToList

            'Indexes info
            Try

                strCommand = "select" & vbNewLine
                strCommand = strCommand & "i.relpages as index," & vbNewLine
                strCommand = strCommand & "i.relname as index_name," & vbNewLine
                strCommand = strCommand & "a.attname as column_name" & vbNewLine
                strCommand = strCommand & "from" & vbNewLine
                strCommand = strCommand & "pg_class t," & vbNewLine
                strCommand = strCommand & "pg_class i," & vbNewLine
                strCommand = strCommand & "pg_index ix," & vbNewLine
                strCommand = strCommand & "pg_attribute a" & vbNewLine
                strCommand = strCommand & "where" & vbNewLine
                strCommand = strCommand & "t.oid = ix.indrelid" & vbNewLine
                strCommand = strCommand & "and i.oid = ix.indexrelid" & vbNewLine
                strCommand = strCommand & "and a.attrelid = t.oid" & vbNewLine
                strCommand = strCommand & "and a.attnum = ANY(ix.indkey)" & vbNewLine
                strCommand = strCommand & "and t.relkind = 'r'" & vbNewLine
                strCommand = strCommand & "and t.relname like '" & p_strTable & "'" & vbNewLine
                strCommand = strCommand & "order by" & vbNewLine
                strCommand = strCommand & "t.relname," & vbNewLine
                strCommand = strCommand & "i.relname" & vbNewLine

                objAuxDataSet = fnExecute(strCommand)

                objReturn.Indexes = (From Register As DataRow In objAuxDataSet.Tables(0).Rows
                                     Where Register("index_name").ToString.Trim.ToUpper.Contains("PKEY") = False
                                     Group By IndexName = Register("index_name") Into Indexes = Group
                                     Select New clsIndexInfo() With {.Name = IndexName,
                                                                     .Columns = (From Register As DataRow In Indexes
                                                                                 Select New clsColumnInfo() With {.Index = Register("index"),
                                                                                                                  .Name = Register("column_name")}).ToList}).ToList

                objReturn.Indexes.ForEach(Sub(index As clsIndexInfo)
                                              index.Columns.ForEach(Sub(Column As clsColumnInfo)
                                                                        Dim objAuxColumn As clsColumnInfo
                                                                        objAuxColumn = objReturn.Columns.Find(Function(Col As clsColumnInfo) Col.Name.Trim.ToUpper = Column.Name.Trim.ToUpper)
                                                                        If objAuxColumn IsNot Nothing Then
                                                                            Column.Type = objAuxColumn.Type
                                                                            Column.Size = objAuxColumn.Size
                                                                        End If
                                                                    End Sub)
                                          End Sub)
            Catch ex As DataBaseException
                If ex.Code <> DataBaseException.enmDataBaseExeptionCode.NotExists Then
                    Throw ex
                End If
            End Try


            Return objReturn

        Catch exBd As DataBaseException
            Throw
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Function
#End Region

#Region "Properties"
    Public Overrides ReadOnly Property isOpen() As Boolean Implements IDataBase.isOpen
        Get
            If objSqlConnection IsNot Nothing Then
                Return objSqlConnection.State <> ConnectionState.Closed
            Else
                Return False
            End If
        End Get
    End Property
#End Region
End Class

