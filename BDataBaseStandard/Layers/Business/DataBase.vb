
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

Public Class DataBase

#Region "Types"
    Public Enum enmDataBaseType
        MsSql
        MySql
        SqLite
        Postgre
    End Enum
#End Region

#Region "Declarations"
    Private Shared objConnections As List(Of IDataBase) = New List(Of IDataBase)
#End Region

#Region "Constructor"
    Private Sub New()

    End Sub
#End Region

#Region "Functions"

    Public Shared Function fnOpenConnection(p_strConnectionString As String, p_enmType As enmDataBaseType) As IDataBase

        Dim objReturn As IDataBase

        Try


            Select Case p_enmType
                Case enmDataBaseType.MsSql
                    objReturn = New DataBase_MSSql(p_strConnectionString)
                Case enmDataBaseType.MySql
                    objReturn = New DataBase_MySql(p_strConnectionString)
                Case enmDataBaseType.SqLite
                    objReturn = New DataBase_Sqlite(p_strConnectionString)
                Case enmDataBaseType.Postgre
                    objReturn = New DataBase_Postgre(p_strConnectionString)
                Case Else
                    Throw New Exception("Invalid Data Base Type.")
            End Select

            objConnections.Add(objReturn)


            Return objReturn

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strConnectionString As String, p_intId As Integer, p_enmType As enmDataBaseType) As IDataBase

        Dim objReturn As IDataBase

        Try

            If p_intId = 0 Then
                If objConnections.Count > 0 Then
                    p_intId = objConnections.Max(Function(conn As IDataBase) conn.ConnectionId) + 1
                Else
                    p_intId = 1
                End If
            End If

            objReturn = objConnections.Find(Function(Conn As IDataBase) Conn.ConnectionId = p_intId)

            If objReturn Is Nothing Then

                Select Case p_enmType
                    Case enmDataBaseType.MsSql
                        objReturn = New DataBase_MSSql(p_strConnectionString, p_intId)
                    Case enmDataBaseType.MySql
                        objReturn = New DataBase_MySql(p_strConnectionString, p_intId)
                    Case enmDataBaseType.SqLite
                        objReturn = New DataBase_Sqlite(p_strConnectionString, p_intId)
                    Case enmDataBaseType.Postgre
                        objReturn = New DataBase_Postgre(p_strConnectionString, p_intId)
                    Case Else
                        Throw New Exception("Invalid Data Base Type.")
                End Select

                objConnections.Add(objReturn)

            End If

            Return objReturn

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strServer As String, p_strDataBase As String, p_strUser As String, p_strPassword As String, p_intId As Integer, p_enmType As enmDataBaseType, p_intConnectionTimeout As Integer) As IDataBase

        Dim objReturn As IDataBase

        Try

            If p_intId = 0 Then
                If objConnections.Count > 0 Then
                    p_intId = objConnections.Max(Function(conn As IDataBase) conn.ConnectionId) + 1
                Else
                    p_intId = 1
                End If
            End If

            objReturn = objConnections.Find(Function(Conn As IDataBase) Conn.ConnectionId = p_intId)

            If objReturn Is Nothing Then

                Select Case p_enmType
                    Case enmDataBaseType.MsSql
                        objReturn = New DataBase_MSSql(p_strServer, p_strDataBase, p_strUser, p_strPassword, p_intId, p_intConnectionTimeout)
                    Case enmDataBaseType.MySql
                        objReturn = New DataBase_MySql(p_strServer, p_strDataBase, p_strUser, p_strPassword, p_intId, p_intConnectionTimeout)
                    Case enmDataBaseType.SqLite
                        objReturn = New DataBase_Sqlite(p_strServer, p_strDataBase, p_strUser, p_strPassword, p_intId, p_intConnectionTimeout)
                    Case enmDataBaseType.Postgre
                        objReturn = New DataBase_Postgre(p_strServer, p_strDataBase, p_strUser, p_strPassword, p_intId, p_intConnectionTimeout)
                    Case Else
                        Throw New Exception("Invalid Data Base Type.")
                End Select

                objConnections.Add(objReturn)

            End If

            Return objReturn

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strServer As String, p_strDataBase As String, p_strUser As String, p_strPassword As String, p_intId As Integer, p_enmType As enmDataBaseType) As IDataBase
        Try
            Return fnOpenConnection(p_strServer, p_strDataBase, p_strUser, p_strPassword, p_intId, p_enmType, 31536000)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strServer As String, p_strDataBase As String, p_strUser As String, p_strPassword As String, p_enmType As enmDataBaseType) As IDataBase
        Try
            Return fnOpenConnection(p_strServer, p_strDataBase, p_strUser, p_strPassword, 0, p_enmType, 31536000)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strServer As String, p_strDataBase As String, p_strUser As String, p_strPassword As String, p_enmType As enmDataBaseType, p_intConnectionTimeout As Integer) As IDataBase
        Try
            Return fnOpenConnection(p_strServer, p_strDataBase, p_strUser, p_strPassword, 0, p_enmType, p_intConnectionTimeout)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_objconfig As clsConfiguration, p_intId As Integer) As IDataBase
        Try
            Return fnOpenConnection(p_objconfig.Server, p_objconfig.DataBase, p_objconfig.User, p_objconfig.Password, p_intId, p_objconfig.Type, p_objconfig.ConnetionTimeout)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_objconfig As clsConfiguration) As IDataBase
        Try
            Return fnOpenConnection(p_objconfig, 0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strConfigPath As String, p_intId As Integer) As IDataBase

        Dim objAuxConfig As clsConfiguration
        Dim objFile As IO.StreamReader

        Try

            If IO.File.Exists(p_strConfigPath) = False Then
                Throw New DataBaseException(DataBaseException.enmDataBaseExeptionCode.Erro, "Configuration file not found: " & p_strConfigPath)
            End If

            objFile = New IO.StreamReader(p_strConfigPath)
            objAuxConfig = clsConfiguration.fnDeserializar(objFile.ReadToEnd)
            objFile.Close()
            objFile.Dispose()

            Return fnOpenConnection(objAuxConfig.Server, objAuxConfig.DataBase, objAuxConfig.User, objAuxConfig.Password, p_intId, objAuxConfig.Type)

        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Function fnOpenConnection(p_strConfigPath As String) As IDataBase
        Try
            Return fnOpenConnection(p_strConfigPath, 0)
        Catch ex As Exception
            Throw
        End Try
    End Function

    Public Shared Sub sbSaveConfiguration(p_objConfiguration As clsConfiguration, p_strPath As String)

        Dim objFile As IO.StreamWriter

        Try

            objFile = New IO.StreamWriter(p_strPath)
            objFile.Write(p_objConfiguration.fnSerializar)
            objFile.Close()
            objFile.Dispose()
            objFile = Nothing

        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub

    Public Shared Sub sbSaveConfiguration(p_objConnection As IDataBase, p_strPath As String)
        Try
            Call sbSaveConfiguration(p_objConnection.fnGetConfiguration, p_strPath)
        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub

    Public Shared Sub sbSaveConfiguration(p_intId As Integer, p_strPath As String)

        Dim objAuxConnection As IDataBase

        Try

            objAuxConnection = objConnections.Find(Function(conn As IDataBase) conn.ConnectionId = p_intId)

            If objAuxConnection IsNot Nothing Then
                Call sbSaveConfiguration(objAuxConnection.fnGetConfiguration, p_strPath)
            Else
                Throw New DataBaseException(DataBaseException.enmDataBaseExeptionCode.Erro, "Id not found:" & p_intId)
            End If

        Catch ex As Exception
            Throw New DataBaseException(ex)
        End Try
    End Sub

    Public Shared Sub sbCloseConnection(p_intId As Integer)

        Dim objAuxConnection As IDataBase

        Try

            objAuxConnection = objConnections.Find(Function(Conn As IDataBase) Conn.ConnectionId = p_intId)

            If objAuxConnection IsNot Nothing Then
                objAuxConnection.sbClose()
                objConnections.Remove(objAuxConnection)
                objAuxConnection = Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Shared Sub sbCloseAllConnections()
        Try

            If objConnections.Count > 0 Then

                objConnections.ForEach(Sub(Conn As IDataBase)
                                           Conn.sbClose()
                                           Conn = Nothing
                                       End Sub)

                objConnections.Clear()

            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Properties"
    Public Shared ReadOnly Property Connections() As List(Of IDataBase)
        Get
            Return objConnections
        End Get
    End Property

    Public Shared ReadOnly Property Connection(p_intId As Integer) As IDataBase
        Get
            Dim objAuxConnection As IDataBase
            Try
                objAuxConnection = objConnections.Find(Function(conn As IDataBase) conn.ConnectionId = p_intId)
                If objAuxConnection IsNot Nothing Then
                    Return objAuxConnection
                Else
                    Throw New DataBaseException(DataBaseException.enmDataBaseExeptionCode.Erro, "Id not found:" & p_intId)
                End If
            Catch ex As Exception
                Throw New DataBaseException(ex)
            End Try
        End Get
    End Property
#End Region

End Class
