# Benford para o Bolsa Familia

Esse trabalho performa a estatística do primeiro dígito de Benford, com a exibição em Excel do modelo teórico. 

### Prerequisitos

Sistema Operacional Linux
MongoDB 3.0 ou superior, Community Edition



## Benford teórico


### Primeiro dígito

![alt text](https://github.com/rodrigoms2004/BenfordBolsaFamilia/blob/master/graficos/benford_primeiro_digito.png)


### Segundo dígito

![alt text](https://github.com/rodrigoms2004/BenfordBolsaFamilia/blob/master/graficos/benford_segundo_digito.png)


### Terceiro dígito

![alt text](https://github.com/rodrigoms2004/BenfordBolsaFamilia/blob/master/graficos/benford_terceiro_digito.png)


## Download dos dados

Faça o download da base do portal transparencia, selecionando o ano e o mês desejado.

http://www.portaltransparencia.gov.br/download-de-dados/bolsa-familia-pagamentos


Faça a extração do arquivo CSV
```
linux_shell:~$ unzip 201701_BolsaFamiliaFolhaPagamento.zip
```

Neste caso o arquivo *201701_BolsaFamiliaFolhaPagamento.csv* ficará disponível.

## Trabalhando os dados

As operações descritas podem ser feitas de modo automático através do script *converter.sh*, 
através do comando:
```
linux_shell:~$ sh converter.sh aquivo_original.csv arquivo_convertido.csv
```
Exemplo:
```
linux_shell:~$ sh converter.sh 201701_BolsaFamiliaFolhaPagamento.csv bolsafamJan2017.csv
```

### Mudando o encoding

Por padrão esse arquivo CSV vem com o enconding *charset=iso-8859-1*.
```
linux_shell:~$ file -i 201701_BolsaFamiliaFolhaPagamento.csv
201701_BolsaFamiliaFolhaPagamento.csv: text/plain; charset=iso-8859-1
```

Mude para *UTF-8* criando o arquivo *bolsafamJan2017.csv*
```
linux_shell:~$ iconv -f ISO-8859-1 -t UTF-8//TRANSLIT 201701_BolsaFamiliaFolhaPagamento.csv -o bolsafamJan2017.csv
```

Verifique se a conversão foi bem sucedida.
```
linux_shell:~$ file -i bolsafamJan2017.csv
bolsafamJan2017.csv: text/plain; charset=utf-8
```

### Removendo acentos, modificando tabulações e ajustando cabeçalhos

Remova os acentos:
```
linux_shell:~$ sed -i 'y/áÁàÀãÃâÂéÉêÊíÍóÓõÕôÔúÚüÜçÇ/aAaAaAaAeEeEiIoOoOoOuUuUcC/' bolsafamJan2017.csv
```

Troque a tabulação de tab por vírgula:
```
linux_shell:~$ sed -i 's/\t/,/g' bolsafamJan2017.csv 
```

Remova os espaços em branco dos cabeçalhos
```
linux_shell:~$ sed -i '1 s/[\t ]//g' bolsafamJan2017.csv
```


## Trabalhando com o MongoDB

Para instruções de instalação visite:
https://docs.mongodb.com/manual/administration/install-community/

### Importando os dados no MongoDB

A importação será feita em um host remoto, caso seja na local, use *localhost* ou *127.0.0.1* como host.

Isso criará a databasee *bolsafamilia2017* com os dados armazenados na collection *janeiro*.
```
mongoimport --host localhost -d bolsafamilia2017 -c janeiro --type csv --headerline --file bolsafamJan2017.csv
```

Para esse arquivo foram importados 13.601.764 documentos
```
2019-07-01T16:37:11.443-0300	connected to: 172.16.105.153
2019-07-01T16:37:14.437-0300	[........................] bolsafamilia2017.janeiro	21.4MB/1.58GB (1.3%)
2019-07-01T16:37:17.437-0300	[........................] bolsafamilia2017.janeiro	41.6MB/1.58GB (2.6%)
...
2019-07-01T16:41:23.437-0300	[#######################.] bolsafamilia2017.janeiro	1.58GB/1.58GB (99.9%)
2019-07-01T16:41:23.703-0300	[########################] bolsafamilia2017.janeiro	1.58GB/1.58GB (100.0%)
2019-07-01T16:41:23.703-0300	imported 13601764 documents
```

### Acessando a base de dados e executando comandos básicos 

Connecte a base atraveś do Mongo Shell 
```
mongo --host <IP do servidor>
```

ou se estiver na máquina local:
```
mongo
```

Execute o comando *show dbs* para mostrar a base de dados:
```
> show dbs
admin             0.000GB
bolsafamilia2017  1.392GB
config            0.000GB
local             0.000GB
```

Selecione a base *bolsafamilia2017* com o comando *use*:
```
> use bolsafamilia2017
switched to db bolsafamilia2017
```

A partir deste momento o cursor *db* apontará para a base de dados *bolsafamilia2017*
```
> db
bolsafamilia2017
```


Liste as collections dentro dessa database, collections seriam o equivalente a tabelas em bancos de dados não relacionais.
```
> show collections
janeiro
```

Valide a quantidade de registros:
```
> db.janeiro.count()
13601764
```

## Benford

### Benford Primeiro dígito

Faz a estatística do primeiro dígito de Benford
```
var arrayBenford = []
for (i = 0; i <=9 ; i++) { arrayBenford[i] = 0 }
db.janeiro.find({}).forEach(function(e) {

	PrimeiroDigito = new String(e.ValorParcela)[0]

	arrayBenford[parseInt(PrimeiroDigito)] += 1

});
```

Exibe o resultado
```
arrayBenford.forEach(function(item, index, array) {
  print("Digito", index,": ",  item);
});
```

Resultados:
```
Digito 0 :  0
Digito 1 :  5814972
Digito 2 :  2975482
Digito 3 :  1656204
Digito 4 :  500093
Digito 5 :  155033
Digito 6 :  37698
Digito 7 :  549806
Digito 8 :  1857489
Digito 9 :  54987
```

![alt text](https://github.com/rodrigoms2004/BenfordBolsaFamilia/blob/master/graficos/benford_primeiro_digito_bolsafamiliaJan2017.png)

### Benford Segundo dígito

Faz a estatística do segundo dígito de Benford
```
var arrayBenford = []
for (i = 0; i <=9 ; i++) { arrayBenford[i] = 0 }
db.janeiro.find({}).forEach(function(e) {

	SegundoDigito = new String(e.ValorParcela)[1]

	arrayBenford[parseInt(SegundoDigito)] += 1

});
```

Exibe o resultado
```
arrayBenford.forEach(function(item, index, array) {
  print("Digito", index,": ",  item);
});
```

Resultados:
```
Digito 0 :  1144017
Digito 1 :  658295
Digito 2 :  2433191
Digito 3 :  611830
Digito 4 :  1062824
Digito 5 :  2408830
Digito 6 :  1839247
Digito 7 :  1459627
Digito 8 :  868974
Digito 9 :  1113764
```

![alt text](https://github.com/rodrigoms2004/BenfordBolsaFamilia/blob/master/graficos/benford_segundo_digito_bolsafamiliaJan2017.png)

### Benford Terceiro dígito

Faz a estatística do terceiro dígito de Benford
```
var arrayBenford = []
for (i = 0; i <=9 ; i++) { arrayBenford[i] = 0 }
db.janeiro.find({}).forEach(function(e) {

	TerceiroDigito = new String(e.ValorParcela)[2]

	arrayBenford[parseInt(TerceiroDigito)] += 1

});
```

Exibe o resultado
```
arrayBenford.forEach(function(item, index, array) {
  print("Digito", index,": ",  item);
});
```

Resultados:
```
Digito 0 :  570821
Digito 1 :  1309889
Digito 2 :  1545157
Digito 3 :  1705594
Digito 4 :  2095787
Digito 5 :  239549
Digito 6 :  837532
Digito 7 :  1137778
Digito 8 :  470071
Digito 9 :  459532
```

![alt text](https://github.com/rodrigoms2004/BenfordBolsaFamilia/blob/master/graficos/benford_terceiro_digito_bolsafamiliaJan2017.png)

### Pagamentos 

Total pago no mês de Janeiro de 2017, **R$ 2.425.388.470,00**
```
db.janeiro.aggregate({
	"$group": {
		"_id": null,
		"total_pago": { "$sum": "$ValorParcela" }
	}
})

{ "_id" : null, "total_pago" : 2425388470 }
```

### Médias e desvio padrão

Valor médio de pagamento por benefeciário, **R$ 178,31**
```
db.janeiro.aggregate({
	"$group": {
		"_id": null,
		"avg_parcela": { "$avg": "$ValorParcela" }
	}
})

{ "_id" : null, "avg_parcela" : 178.3142590916884 }
```

Obtém o desvio padrão da população *n* 
```
db.janeiro.aggregate({
	"$group": {
		"_id": null,
		"desvio_parcela": { "$stdDevPop": "$ValorParcela" }
	}
})

{ "_id" : null, "desvio_parcela" : 104.05486449842934 }
```

Obtém o desvio padrão da amostra  *(n-1)* 
```
db.janeiro.aggregate({
	"$group": {
		"_id": null,
		"desvio_parcela": { "$stdDevSamp": "$ValorParcela" }
	}
})

{ "_id" : null, "desvio_parcela" : 104.0548683234799 }
```

Com um desvio padrão de **R$ 104,05**, signfica que os valores pagos aos beneficiários do programa 
variam entre **R$ 74,26** e **R$ 282,36**, ou seja: **R$ 178,31 +/- R$ 104,05**.

### Pagamentos máximos e mínimos.

O maior valor pago foi de **R$ 998,00**.
```
db.janeiro.aggregate({
	"$group": {
		"_id": null,
		"maximo": { "$max": "$ValorParcela" }
	}
})

{ "_id" : null, "maximo" : 998 }
```

Enquanto o menor valor pago foi de **R$ 1,00**
```
db.janeiro.aggregate({
	"$group": {
		"_id": null,
		"minimo": { "$min": "$ValorParcela" }
	}
})

{ "_id" : null, "minimo" : 1 }
```

### Busca por intervalos de benefícios 

Obtém o número de beneficiários que recebem entre **R$ 100,00** e **R$ 199,00**.
```
db.janeiro.find({ValorParcela: {$gte: 100, $lte: 199}}, {_id: 0, ValorParcela: 1}).count()

5813816
```

Neste grupo são 5.813.816 de beneficiários.

obtém a média entre os valores de R$ 100,00 e R$ 199,00
```
db.jan.aggregate([
	{$match : {
	$and : [
	{"ValorParcela" : {$gte: 100}},
	{"ValorParcela" : {$lte: 199}}
	]
	}},
	    {$group : {
	        _id : null,
	        media : {$avg: "$ValorParcela"}
	    }}
	])
{ "_id" : null, "media" : 142.4 }
```

Nesse grupo o valor médio recebido é de **R$ 142,00**


### Playground

Separa os digitos em collections distintas.
```
var arrayBenford = []
for (i = 0; i <=9 ; i++) { arrayBenford[i] = 0 }
db.janeiro.find({}).forEach(function(e) {
    
    PrimeiroDigito = new String(e.ValorParcela)[0]

//    arrayBenford[parseInt(PrimeiroDigito)] += 1
    
    switch (parseInt(PrimeiroDigito)) {
        case 0: 
        break;
        case 1:
            db.digito1.insert(e)
        break;
        case 2:
            db.digito2.insert(e)
        break;
        case 3:
            db.digito3.insert(e)
        break;
        case 4:
            db.digito4.insert(e)
        break;
        case 5:
            db.digito5.insert(e)
        break;
        case 6:
            db.digito6.insert(e)
        break;
        case 7:
            db.digito7.insert(e)
        break;
        case 8:
            db.digito8.insert(e)
        break;
        case 9:
            db.digito9.insert(e)
        break;
    }
});
```

## Benford Teórico VBA

```
Option Explicit

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
```

## Autor

* **Rodrigo Moraes Silveira**
*Git hub* - (https://github.com/rodrigoms2004)
*E-mail*  - rodrigoms2004@gmail.com

## Licença

Copyfree RMSMath
