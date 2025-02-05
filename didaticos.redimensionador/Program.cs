using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace didaticos.redimensionador
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("iniciando nosso redimensionador");
            Thread thread = new Thread(Redimensionar);
            thread.Start();



            Console.Read();
        }

        static void Redimensionar()
        {
            string arquivo_entrada = "Arquivos_Entrada";
            string arquivo_redimensionado = "Arquivos_Redimensionados";
            string arquivo_finalizado = "Arquivos_Finalizados";

            if (!Directory.Exists(arquivo_entrada))
            {
                Directory.CreateDirectory(arquivo_entrada);
            }


            if (!Directory.Exists(arquivo_redimensionado))
            {
                Directory.CreateDirectory(arquivo_redimensionado);
            }


            if (!Directory.Exists(arquivo_finalizado))
            {
                Directory.CreateDirectory(arquivo_finalizado);
            }

            FileStream fileStream;
            FileInfo fileInfo;

            while (true)
            {
                //Meu programa vai olhar para a pasta de entrada
                //Se tiver arquivo, ele irá redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(arquivo_entrada);

                int tamanho = 200;

                //Ler o tamanho do arquivo a ser redimensionado

                foreach (var arquivo in arquivosEntrada)
                {
                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + arquivo_redimensionado + @"\" + fileInfo.Name + DateTime.Now.Millisecond.ToString();

                    //Redimensiona - //copia os arquivos redimensionados para a pasta de Arquivos_Redimensionados
                    Redimensionador(Image.FromStream(fileStream), tamanho, caminho);

                    //fecha o arquivo
                    fileStream.Close();

                    //Move o arquivo para a pasta Arquivos_Finalizados
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + arquivo_finalizado + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(caminhoFinalizado);

                    
                }


                Thread.Sleep(new TimeSpan(0, 0, 5));
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que vamos redimensionar</param>
        /// <param name="caminho">Caminho para gravar o arquivo redimensionado</param>
        /// <returns></returns>

        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            // double ratio = Convert.ToDouble(altura / imagem.Height);
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);
            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }

            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }
}