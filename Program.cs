// See https://aka.ms/new-console-template for more information
using System.Text;
using System;
using System.Collections.Generic;
using Moq;

public class Program
{
    public class Marcacao
    {
        public string Cpf { get; set; }               // String
        public DateTime DateTimeMarkingPoint { get; set; } // DateTime
        public string NSR { get; set; }               // String
    }
    private static void Main(string[] args)
    {
        var marcacoes = new List<Marcacao>();

        for (int i = 0; i < 10; i++)
        {
            marcacoes.Add(new Marcacao
            {
                Cpf = "02938870043",
                DateTimeMarkingPoint = new DateTime(2024, i + 1, 1), // Ajustar mês e dia
                NSR = $"{i + 1}" // Gerar NSR sequencial
            });
        }


        string[] lines = File.ReadAllLines("PastaDeBatidas\\afd.txt");
        StringBuilder afd = new StringBuilder();
        StringBuilder afdCabecalho = new StringBuilder();

        afdCabecalho.Append("000000000");//Espaço antes do conteúdo do cabeçalho
        afdCabecalho.Append("1");// Tipo de registo. Cabeçalho = 1
        afdCabecalho.Append("1");// Tipo de Identificador do empregador. Cnpj = 1, Cpf = 2
        afdCabecalho.Append(123456789);//Cnpj da empresa
        afdCabecalho.Append("000000000000");//CNO ou CAEPF se existir
        afdCabecalho.Append("PREFEITURA".PadRight(150));//Razão Social- 150 caracteres por padrão
        afdCabecalho.Append("12345678912345");//Razão Social- 150 caracteres por padrão
        afdCabecalho.Append("01012024"); //Data do primeiro registro do arquivo
        afdCabecalho.Append("10012024"); //Data do ultimo registro do arquivo
        afdCabecalho.Append(DateTime.Now.ToString("ddMMyyyyHH")); //Data e hora da geração do arquivo
        afdCabecalho.Append("03");// versão de layout do afd. padrão 003
        if (lines.Length == 0)
        {
            afdCabecalho.Append('\n');
        }


        using (StreamWriter writer = new StreamWriter("PastaDeBatidas\\afd.txt"))
        {
            if (lines.Length == 0)
            {
                writer.WriteLine(afdCabecalho);
            }

            for (int i = 0; i < lines.Length; i++)
            {
                if (i == 0)
                {
                    writer.WriteLine(afdCabecalho);
                }
                else
                {
                    writer.WriteLine(lines[i]);
                }
            }
        }


        for (var i = 0; i < marcacoes.Count; ++i)
        {
            StringBuilder afdBatidas = new StringBuilder();
            afdBatidas.Append(marcacoes[i].NSR.PadLeft(9, '0'));//NSR da batida
            afdBatidas.Append('3');//padrão de layout
            afdBatidas.Append(marcacoes[i].DateTimeMarkingPoint.ToString("ddMMyyyyHHmm"));//Data e hora da marcacao
            afdBatidas.Append(marcacoes[i].Cpf.PadLeft(12, '0')); //retorna pis pelo dicionario retornado do gespam
            if (i != marcacoes.Count - 1)
            {
                afdBatidas.Append('\n');
            }
            afd.Append(afdBatidas);
        }

        using (StreamWriter writer = new StreamWriter("PastaDeBatidas\\afd.txt", true))
        {
            writer.WriteLine(afd.ToString());
        }

    }

}