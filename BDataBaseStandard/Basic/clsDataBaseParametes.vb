
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

Public Class clsDataBaseParametes

#Region "Declarations"
    Private strKey As String
    Private objValue As Object
#End Region

#Region "Constructors"
    Public Sub New()
        Try
            strKey = ""
            objValue = Nothing
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Public Sub New(p_strKey As String, p_strValue As Object)
        Try
            strKey = p_strKey
            objValue = p_strValue
        Catch ex As Exception
            Throw
        End Try
    End Sub
#End Region

#Region "Propeties"
    Public Property Key() As String
        Get
            Return strKey
        End Get
        Set(ByVal value As String)
            strKey = value
        End Set
    End Property
    Public Property Value() As Object
        Get
            Return objValue
        End Get
        Set(ByVal value As Object)
            objValue = value
        End Set
    End Property
#End Region

End Class
