Option Explicit

' Benford Te�rico por Rodrigo Moraes Silveira
' 01.05.2018 - rodrigo.silveira@rmsmath.com.br / rodrigoms2004@gmail.com
' uso livre

Sub Benford_Teorico()

    Call criaLayout
    
    Dim i As Long
    i = 0
    
    ' popula coluna N�mero de 0 at� 0
    Call populaDigitos
    
    ' Gera o Benford do Primero D�gito
    ' de 0 a 9
    Call benford_PrimeiroDigito
    
    ' Gera o Benford do Segundo D�gito
    ' de 10 a 99
    Call benford_SegundoDigito
    
    Dim segundoDigito, p_somas, s_somas As Long
    Dim soma As Double
    
    ' Gera o Benford do Terceiro D�gito
    ' de 100 a 999
    Call benford_TerceiroDigito
    
    Dim terceiroDigito, t_somas As Long
    
    ' Gera o Benford do Quarto D�gito
    ' de 1000 a 9999
    Call benford_QuartoDigito

End Sub

Sub criaLayout()

    Sheets(1).Name = Benford
    Sheets(Benford).Select
    Range(A1).Select
    
    ActiveCell = N�mero
    ActiveCell.Offset(0, 1).Select
    ActiveCell = Primeiro D�gito
    ActiveCell.Offset(0, 1).Select
    ActiveCell = Segundo D�gito
    ActiveCell.Offset(0, 1).Select
    ActiveCell = Terceiro D�gito
    ActiveCell.Offset(0, 1).Select
    ActiveCell = Quarto D�gito
    Range(A1).Select
    
    Columns(AA).EntireColumn.AutoFit
    Columns(BB).EntireColumn.AutoFit
    Columns(CC).EntireColumn.AutoFit
    Columns(DD).EntireColumn.AutoFit
    Columns(EE).EntireColumn.AutoFit
    
        ' Coloca em formato de porcentagem
    ' com dois d�gitos ap�s a v�rgula
    Range(B2E11).Select
    Selection.Style = Percent
    Selection.NumberFormat = 0.00%
    
End Sub

Sub populaDigitos()

    Dim i As Long
    i = 0
    Do While i  10
        Cells(i + 2, 1) = i
        i = i + 1
    Loop
End Sub

Sub benford_PrimeiroDigito()
    Dim p_somas As Long
    p_somas = 0
    Do While p_somas  10
        If (Cells(p_somas + 2, 1) = 0) Then
            Cells(p_somas + 2, 2) = 0
        Else
            Cells(p_somas + 2, 2) = _
                Application. _
                WorksheetFunction. _
                Log10(1 + 1  Cells(p_somas + 2, 1))
        End If
        p_somas = p_somas + 1
    Loop
End Sub
    
Sub benford_SegundoDigito()
    Dim segundoDigito, p_somas, s_somas As Long
    Dim soma As Double
    
    s_somas = 0
    Do While s_somas  10
        p_somas = 0
        soma = 0
        Do While p_somas  9
            segundoDigito = Cells(p_somas + 3, 1) & _
                            Cells(s_somas + 2, 1)
            ' soma Log10(1 + 110) + ... + Log10(1 + 190)
            soma = soma + Application. _
                          WorksheetFunction. _
                          Log10(1 + 1  segundoDigito)
            p_somas = p_somas + 1
        Loop
        Cells(s_somas + 2, 3) = soma
        s_somas = s_somas + 1
    Loop
End Sub

Sub benford_TerceiroDigito()
    Dim segundoDigito, terceiroDigito, _
        p_somas, s_somas, t_somas As Long
    Dim soma As Double

    t_somas = 0
    Do While t_somas  10
        s_somas = 0
        soma = 0
        Do While s_somas  10
            p_somas = 0
            Do While p_somas  9
                terceiroDigito = Cells(p_somas + 3, 1) & _
                                Cells(s_somas + 2, 1) & _
                                Cells(t_somas + 2, 1)
                soma = soma + Application. _
                WorksheetFunction. _
                Log10(1 + 1  terceiroDigito)
                p_somas = p_somas + 1
            Loop
            s_somas = s_somas + 1
        Loop
        Cells(t_somas + 2, 4) = soma
        t_somas = t_somas + 1
    Loop
End Sub

Sub benford_QuartoDigito()
    Dim segundoDigito, terceiroDigito, _
        quartoDigito, _
        p_somas, s_somas, t_somas, _
        q_somas As Long
    Dim soma As Double
    
    q_somas = 0
    Do While q_somas  10
        t_somas = 0
        soma = 0
        Do While t_somas  10
            s_somas = 0
            Do While s_somas  10
                p_somas = 0
                Do While p_somas  9
                    quartoDigito = Cells(p_somas + 3, 1) & _
                                    Cells(s_somas + 2, 1) & _
                                    Cells(t_somas + 2, 1) & _
                                    Cells(q_somas + 2, 1)
                    soma = soma + Application. _
                    WorksheetFunction. _
                    Log10(1 + 1  quartoDigito)
                    p_somas = p_somas + 1
                Loop
                s_somas = s_somas + 1
            Loop
            t_somas = t_somas + 1
        Loop
        Cells(q_somas + 2, 5) = soma
        q_somas = q_somas + 1
    Loop
End Sub


