
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


Imports MySql.Data.MySqlClient
Imports System.Data
Imports System.Data.SQLite

Public Class DataBaseException
    Inherits Exception

#Region "Types"
    Public Enum enmDataBaseExeptionCode
        OK = 0
        ConnectionClosed = 1
        Duplicated = 22
        NotExists = 23
        Erro = 99
    End Enum
#End Region

#Region "Declarations"
    Private enmCode As enmDataBaseExeptionCode
    Private strMessage As String
#End Region

#Region "Constructors"
    Public Sub New(ByVal p_objExecption As Exception)
        MyBase.New(p_objExecption.Message, p_objExecption)
        Try
            enmCode = enmDataBaseExeptionCode.Erro
            strMessage = p_objExecption.Message
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal p_objSqlExecption As SqlClient.SqlException)

        MyBase.New(p_objSqlExecption.Message, p_objSqlExecption)

        Try
            Select Case p_objSqlExecption.ErrorCode
                Case "-2146232060", "-2147467259"
                    If p_objSqlExecption.Message.Trim.ToUpper.IndexOf("DUPLICATE KEY") > -1 Or p_objSqlExecption.Message.Trim.ToUpper.IndexOf("CHAVE DUPLICADA") > -1 Then
                        enmCode = enmDataBaseExeptionCode.Duplicated
                    Else
                        enmCode = enmDataBaseExeptionCode.Erro
                    End If
                Case Else
                    enmCode = enmDataBaseExeptionCode.Erro
            End Select

            strMessage = p_objSqlExecption.Message

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Sub New(ByVal p_objSqlExecption As SqlClient.SqlException, ByVal p_strMessage As String)

        MyBase.New(p_strMessage.Trim, p_objSqlExecption)

        Try
            Select Case p_objSqlExecption.ErrorCode
                Case "-2146232060", "-2147467259"
                    If p_objSqlExecption.Message.Trim.ToUpper.IndexOf("DUPLICATE KEY") > -1 Or p_objSqlExecption.Message.Trim.ToUpper.IndexOf("CHAVE DUPLICADA") > -1 Then
                        enmCode = enmDataBaseExeptionCode.Duplicated
                    Else
                        enmCode = enmDataBaseExeptionCode.Erro
                    End If
                Case Else
                    enmCode = enmDataBaseExeptionCode.Erro
                    strMessage = p_strMessage
            End Select

            strMessage = p_objSqlExecption.Message

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal p_objSqlExecption As MySqlException)

        MyBase.New(p_objSqlExecption.Message, p_objSqlExecption)

        Try
            Select Case p_objSqlExecption.ErrorCode
                Case "-2146232060", "-2147467259"
                    enmCode = enmDataBaseExeptionCode.Duplicated
                Case Else
                    enmCode = enmDataBaseExeptionCode.Erro
            End Select

            strMessage = p_objSqlExecption.Message
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal p_objSqlExecption As MySqlException, ByVal p_sMensagem As String)

        MyBase.New(p_sMensagem, p_objSqlExecption)
        Try
            Select Case p_objSqlExecption.ErrorCode
                Case "-2146232060", "-2147467259"
                    enmCode = enmDataBaseExeptionCode.Duplicated
                Case Else
                    enmCode = enmDataBaseExeptionCode.Erro
            End Select

            strMessage = p_sMensagem
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal p_objSqlExecption As SQLiteException)

        MyBase.New(p_objSqlExecption.Message, p_objSqlExecption)

        Try
            Select Case p_objSqlExecption.ErrorCode
                Case "19"
                    enmCode = enmDataBaseExeptionCode.Duplicated
                Case Else
                    enmCode = enmDataBaseExeptionCode.Erro
            End Select
            strMessage = p_objSqlExecption.Message
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal p_objSqlExecption As SQLiteException, ByVal p_sMensagem As String)

        MyBase.New(p_sMensagem, p_objSqlExecption)
        Try
            Select Case p_objSqlExecption.ErrorCode
                Case "-2146232060", "-2147467259"
                    enmCode = enmDataBaseExeptionCode.Duplicated
                Case Else
                    enmCode = enmDataBaseExeptionCode.Erro
            End Select
            strMessage = p_sMensagem
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Sub New(ByVal p_enmCode As enmDataBaseExeptionCode, ByVal p_strMensagem As String)
        Try
            enmCode = p_enmCode
            strMessage = p_strMensagem
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Properties"
    Public Property Code() As enmDataBaseExeptionCode
        Get
            Return enmCode
        End Get

        Set(ByVal p_enmCode As enmDataBaseExeptionCode)

        End Set
    End Property

    Public Overrides ReadOnly Property Message As String
        Get
            Return strMessage
        End Get
    End Property

#End Region

End Class