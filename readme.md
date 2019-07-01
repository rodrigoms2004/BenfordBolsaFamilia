# Benford para o Bolsa Familia

Esse trabalho performa a estatística do primeiro dígito de Benford, com a exibição em Excel do modelo teórico. 

### Prerequisites

Sistema Operacional Linux
MongoDB 3.0 ou superior, Community Edition


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


## Authors

* **Rodrigo Moraes Silveira**
*Git hub* - (https://github.com/rodrigoms2004)
*E-mail*  - rodrigoms2004@gmail.com

## License

Copyright RMSMath