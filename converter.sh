#!/bin/bash

# Mude para *UTF-8* criando o arquivo *bolsafamJan2017.csv*
iconv -f ISO-8859-1 -t UTF-8//TRANSLIT $1 -o $2

# Remova os acentos:
sed -i 'y/áÁàÀãÃâÂéÉêÊíÍóÓõÕôÔúÚüÜçÇ/aAaAaAaAeEeEiIoOoOoOuUuUcC/' $2

# Troque a tabulação de tab por vírgula:
sed -i 's/\t/,/g' $2 

# Remova os espaços em branco dos cabeçalhos
sed -i '1 s/[\t ]//g' $2

