using System;
using System.Globalization;

namespace DifDatas
{
    class saida
    {
        static void Main()
        {
            Console.WriteLine("Formato de data: dd/mm/yyyy");

            Data dataInicial = new Data();

            string dataStringInicial = string.Empty;
            do
            {
                Console.Write("Informe a data inicial: ");
                dataStringInicial = Console.ReadLine();
            } while (!dataInicial.setData(dataStringInicial));

            bool isValidDtFinal = false;
            Data dataFinal = new Data();
            do
            {
                Console.Write("Informe a data final: ");
                string dataStringFinal = Console.ReadLine();
                Data dtTemp = new Data();

                if (!dtTemp.setData(dataStringFinal))
                {
                    continue;
                }

                if (dtTemp < dataInicial)
                {
                    continue;
                }
                else
                {
                    isValidDtFinal = true;
                    dataFinal.setData(dataStringFinal);
                }
            }

            while (!isValidDtFinal);


            //DateTime dateInicial = DateTime.Parse(dateStringInicial, CultureInfo.GetCultureInfo("pt"));
            // DateTime dateFinal = DateTime.Parse(dateStringFinal, CultureInfo.GetCultureInfo("pt"));


            //TimeSpan dateSubtr = dateFinal.Subtract(dateInicial);

            Console.WriteLine("Está faltando: ");

            int difDia = 0;
            int difMes = 0;
            int difAno = 0;
            if (dataInicial.dia > dataFinal.dia)
            {
                //calculando os dias
                difDia = dataInicial.mesMaxDias[dataInicial.mes -1] - dataInicial.dia;
                difDia += dataFinal.dia;

                //pensamentos errados, que fugiram do resultado em algumas situaçoes
               // difDia += dataFinal.mesMaxDias[dataFinal.mes - 1];
              //  difDia -= dataInicial.dia;


                //aumento um mes ao valor do mes inicial
                int inicialUpdateMes = dataInicial.mes+1;

                if (dataFinal.mes < inicialUpdateMes)
                {
                    difMes = 12 - inicialUpdateMes + dataFinal.mes;
                    difAno =  dataFinal.ano - (dataInicial.ano + 1);
                }

                else
                {
                    difMes = dataFinal.mes - inicialUpdateMes;
                    difAno = dataFinal.ano - dataInicial.ano;
                }

            }

            else
            {
                difDia = dataFinal.dia - dataInicial.dia;
                if (dataInicial.mes > dataFinal.mes)
                {
                    difMes = 12 - dataInicial.mes + dataFinal.mes;
                    difAno = dataFinal.ano -  (dataInicial.ano + 1);
                }

                else{
                    difMes = dataFinal.mes - dataInicial.mes;
                    difAno = dataFinal.ano - dataInicial.ano;
                }
                
            }

            Console.WriteLine("Anos: {0}\nMeses: {1}\nDias: {2}", difAno, difMes, difDia);
            Console.WriteLine("----------Dados gerais-------\nTotal Dias: {0}\nTotal Meses: {1}\n\nObrigado!",
            dataFinal.TotalDias - dataInicial.TotalDias, (dataFinal.TotalMeses-dataInicial.TotalMeses).ToString("F2"));
    

            Console.ReadKey();

        }

        class Data
        {
            public int ano;
            public int dia;
            public int mes;

            public long TotalDias = 0;
            public double TotalMeses= 0;

            public int[] mesMaxDias = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            public Data() { }

            public Data(int dia, int mes, int ano)
            {
                this.ano = ano;
                this.mes = mes;
                this.dia = dia;
            }

            public bool setData(string DataFormated)
            {

                if (DataFormated.Length != 10)
                {
                    return false;
                }
                string dia = DataFormated.Substring(0, 2);

                if (!isNumber(dia))
                {
                    return false;
                }

                if (DataFormated[2] != '/')
                {
                    return false;
                }

                string mes = DataFormated.Substring(3, 2);
                if (!isNumber(mes))
                {
                    return false;
                }

                if (DataFormated[5] != '/')
                {
                    return false;
                }

                string ano = DataFormated.Substring(6, 4);

                if (!isNumber(ano))
                {
                    return false;
                }

                if (int.Parse(ano) % 4 == 0)
                {
                    mesMaxDias[1] += 1;
                }

                if (int.Parse(mes) < 1 || int.Parse(mes) > 12)
                {
                    return false;
                }

                if (int.Parse(dia) > mesMaxDias[int.Parse(mes) - 1] || int.Parse(dia) == 0)
                {
                    return false;
                }


                this.dia = int.Parse(dia);
                this.mes = int.Parse(mes);
                this.ano = int.Parse(ano);

                CalculateDias();

                return true;

            }


            private void CalculateDias()
            {
                this.TotalDias = 0;

                //calculo do total de meses
                this.TotalMeses = this.mes-1 + ((double) (this.dia-1) / (double)this.mesMaxDias[this.mes-1]) + this.ano * 12;
                for (int x = 1; x < this.ano; x++)
                {
                    if (x % 4 == 0)
                    {
                        this.TotalDias += 366;
                    }

                    else
                    {
                        this.TotalDias += 365;
                    }
                }

                for (int x = 1; x < this.mes; x++)
                {
                    this.TotalDias += mesMaxDias[x - 1];
                }

                this.TotalDias += this.dia;
            }

            private bool isNumber(string number)
            {
                try
                {
                    int.Parse(number);
                }
                catch (Exception)
                {
                    return false;
                }

                return true;
            }

            public static bool operator >(Data a, Data b)
            {
                if (a.ano > b.ano)
                {
                    return true;
                }

                else if (a.ano == b.ano)
                {
                    if (a.mes > b.mes)
                    {
                        return true;
                    }

                    else if (a.mes == b.mes)
                    {
                        if (a.dia > b.dia)
                        {
                            return true;
                        }

                        else if (a.dia == b.dia)
                        {
                            return false;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    else
                    {
                        return false;
                    }


                }
                else
                {
                    return false;
                }
            }

            public static bool operator >=(Data a, Data b)
            {
                if (a.ano > b.ano)
                {
                    return true;
                }

                else if (a.ano == b.ano)
                {
                    if (a.mes > b.mes)
                    {
                        return true;
                    }

                    else if (a.mes == b.mes)
                    {
                        if (a.dia > b.dia)
                        {
                            return true;
                        }

                        else if (a.dia == b.dia)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                    else
                    {
                        return false;
                    }


                }
                else
                {
                    return false;
                }
            }

            public static bool operator <=(Data a, Data b)
            {
                if(a < b || a == b){
                    return true;
                }

                return false;
            }

            public static bool operator <(Data a, Data b)
            {
                if (!(a >= b))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            public static bool operator ==(Data a, Data b)
            {
                if (a.dia == b.dia && a.mes == b.mes && a.ano == b.ano)
                {
                    return true;
                }

                return false;
            }

            public static bool operator !=(Data a, Data b)
            {
                if (!(a == b))
                {
                    return true;
                }

                return false;
            }


        }
    }
}