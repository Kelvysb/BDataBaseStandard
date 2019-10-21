
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

Imports Newtonsoft.Json

''' <summary>
''' Classe clsConfiguration
''' Autor: 
''' Data: 21/03/2017
''' Finalidade: Data base configuration class
''' Cliente: 
''' Atividade: 
''' </summary>
''' <remarks></remarks>
Public Class clsConfiguration

#Region "Declarações"
    Private strServer As String
    Private strDataBase As String
    Private strUser As String
    Private strPassword As String
    Private intType As DataBase.enmDataBaseType
    Private intConnectionTimeout As Integer
#End Region

#Region "Contrutores"
    Public Sub New()
        Try
            strServer = ""
            strDataBase = ""
            strUser = ""
            strPassword = ""
            intType = BDataBaseStandard.DataBase.enmDataBaseType.MsSql
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub New(p_xmlEntrada As XDocument)
        Try
            Call sbMontaPorXml(p_xmlEntrada)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region

#Region "Funçoes e Subrotinas"
    ''' <summary>
    ''' Converte o objeto em Xml
    ''' </summary>
    ''' <remarks></remarks>
    Public Function fnToXml() As XDocument
        Dim Retorno As XDocument
        Try

            Retorno = New XDocument(<Configuration></Configuration>)
            Retorno.<Configuration>.First.Add(<Server><%= strServer.Trim %></Server>)
            Retorno.<Configuration>.First.Add(<DataBase><%= strDataBase.Trim %></DataBase>)
            Retorno.<Configuration>.First.Add(<User><%= strUser.Trim %></User>)
            Retorno.<Configuration>.First.Add(<Password><%= strPassword.Trim %></Password>)
            Retorno.<Configuration>.First.Add(<Type><%= intType.ToString.ToUpper %></Type>)
            Return Retorno

        Catch Ex As Exception
            Throw Ex
        End Try
    End Function

    ''' <summary>
    ''' Cria o objeto apartir de um xml
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub sbMontaPorXml(p_xmlEntrada As XDocument)
        Try

            strServer = p_xmlEntrada.<Configuration>.<Server>.Value
            strDataBase = p_xmlEntrada.<Configuration>.<DataBase>.Value
            strUser = p_xmlEntrada.<Configuration>.<User>.Value
            strPassword = p_xmlEntrada.<Configuration>.<Password>.Value
            intType = [Enum].Parse(GetType(DataBase.enmDataBaseType), p_xmlEntrada.<Configuration>.<Type>.Value, True)

        Catch Ex As Exception
            Throw Ex
        End Try
    End Sub

    ''' <summary>
    ''' Cria um clone do objeto
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Clone() As clsConfiguration
        Try

            Return New clsConfiguration(fnToXml)

        Catch Ex As Exception
            Throw Ex
        End Try
    End Function

    ''' <summary>
    ''' Serializa o objeto no formato JSON
    ''' </summary>
    ''' <remarks></remarks>
    Public Function fnSerializar() As String
        Dim Retorno As String
        Try
            Retorno = JsonConvert.SerializeObject(Me, Formatting.Indented)
            Return Retorno
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Serializa o objeto no formato JSON
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function fnSerializar(p_objclsConfiguration As clsConfiguration) As String
        Dim Retorno As String
        Try
            Retorno = JsonConvert.SerializeObject(p_objclsConfiguration, Formatting.Indented)
            Return Retorno
        Catch ex As Exception
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Deserializa o objeto a partir do formato JSON
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Function fnDeserializar(p_strJson As String) As clsConfiguration
        Dim Retorno As clsConfiguration
        Try
            Retorno = JsonConvert.DeserializeObject(Of clsConfiguration)(p_strJson)
            Return Retorno
        Catch ex As Exception
            Throw
        End Try
    End Function


#End Region

#Region "Propriedades"
    ''' <summary>
    ''' Server
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <JsonProperty("SERVER")>
    Public Property Server() As String
        Get
            Return strServer
        End Get
        Set(ByVal p_strServer As String)
            strServer = p_strServer
        End Set
    End Property

    ''' <summary>
    ''' Data base name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <JsonProperty("DATABASE")>
    Public Property DataBase() As String
        Get
            Return strDataBase
        End Get
        Set(ByVal p_strDataBase As String)
            strDataBase = p_strDataBase
        End Set
    End Property

    ''' <summary>
    ''' User name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <JsonProperty("USER")>
    Public Property User() As String
        Get
            Return strUser
        End Get
        Set(ByVal p_strUser As String)
            strUser = p_strUser
        End Set
    End Property

    ''' <summary>
    ''' Password
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <JsonProperty("PASSWORD")>
    Public Property Password() As String
        Get
            Return strPassword
        End Get
        Set(ByVal p_strPassword As String)
            strPassword = p_strPassword
        End Set
    End Property

    ''' <summary>
    ''' Data base type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <JsonProperty("TYPE")>
    Public Property Type() As DataBase.enmDataBaseType
        Get
            Return intType
        End Get
        Set(ByVal p_intType As DataBase.enmDataBaseType)
            intType = p_intType
        End Set
    End Property

    ''' <summary>
    ''' Connection timeout
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <JsonProperty("CONNETIONTIMEOUT")>
    Public Property ConnetionTimeout() As Integer
        Get
            Return intConnectionTimeout
        End Get
        Set(ByVal value As Integer)
            intConnectionTimeout = value
        End Set
    End Property

#End Region

End Class
