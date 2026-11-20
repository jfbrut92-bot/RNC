using System.IO;

using ICSharpCode.SharpZipLib.Zip;

namespace Utac.Rnc2.UI.Web.Utils
{
    public static class Zip
    {
        public static Stream Generate(ZipParameter zipParameter)
        {
            var stream = new MemoryStream();
            using (var zip = new ZipOutputStream(stream))
            {
                zip.SetLevel(5);

                zip.CreateEntry(zipParameter.NomFichierPfx, zipParameter.CertificatPfx, zipParameter.MotPasseFichierZip);
                zip.CreateEntry(Path.GetFileName(zipParameter.CheminFichierCertificatRacine),
                    File.ReadAllBytes(zipParameter.CheminFichierCertificatRacine), zipParameter.MotPasseFichierZip);

                // Stop ZipStream.Dispose() from also Closing the underlying stream
                zip.IsStreamOwner = false;
            }

            // Reset Stream Position
            stream.Position = 0;

            return stream;
        }

        private static void CreateEntry(this ZipOutputStream stream, string fileName, byte[] data,
            string password = null)
        {
            var entry = new ZipEntry(fileName);

            // Security
            if (!string.IsNullOrWhiteSpace(password))
            {
                entry.IsCrypted = true;
                stream.Password = password;
            }

            // Data
            stream.PutNextEntry(entry);
            stream.Write(data, 0, data.Length);
            stream.CloseEntry();
        }
    }
}
