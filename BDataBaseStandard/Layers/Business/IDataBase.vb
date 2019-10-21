
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

Public Interface IDataBase

#Region "Function and SubRoutines"
    Sub sbOpen()
    Sub sbClose()
    Sub sbBegin()
    Sub sbCommit()
    Sub sbRollBack()

    Sub sbExecute(p_strCommand As String)
    Sub sbExecute(p_strCommand As String, ByVal p_intTimeout As Integer)
    Sub sbExecute(p_strCommand As String, ByVal p_objParametros As List(Of clsDataBaseParametes))
    Sub sbExecute(p_strCommand As String, ByVal p_intTimeout As Integer, ByVal p_objParametros As List(Of clsDataBaseParametes))


    Function fnExecute(p_strCommand As String) As DataSet
    Function fnExecute(ByVal p_strCommand As String, ByVal p_intTimeout As Integer) As DataSet
    Function fnExecute(ByVal p_strCommand As String, ByVal p_intTimeout As Integer, p_objParameters As List(Of clsDataBaseParametes)) As DataSet
    Function fnExecute(ByVal p_strCommand As String, p_objParameters As List(Of clsDataBaseParametes)) As DataSet


    Function fnExecute(Of T)(p_strCommand As String) As List(Of T)
    Function fnExecute(Of T)(ByVal p_strCommand As String, ByVal p_intTimeout As Integer) As List(Of T)
    Function fnExecute(Of T)(ByVal p_strCommand As String, p_objParameters As List(Of clsDataBaseParametes)) As List(Of T)
    Function fnExecute(Of T)(ByVal p_strCommand As String, ByVal p_intTimeout As Integer, p_objParameters As List(Of clsDataBaseParametes)) As List(Of T)

    Function fnGetTableInfo(p_strTable As String) As clsTableInfo
    Function fnGetConfiguration() As clsConfiguration
#End Region

#Region "Properties"
    ReadOnly Property Server() As String
    ReadOnly Property DataBase() As String
    ReadOnly Property User() As String
    ReadOnly Property Password() As String
    ReadOnly Property isOpen As Boolean
    ReadOnly Property ConnectionId() As Integer
#End Region

End Interface
