using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using System.IO;
using SistemaReclutamiento.Entidades.BoletasGDT;
using System.Text;

namespace SistemaReclutamiento.Utilitarios
{
    public class Firmante
    {
        private readonly Certificado certificado;

        public Firmante(Certificado certificado)
        {
            this.certificado = certificado;
        }

        public void Firmar(string rutaDocumentoSinFirma, string rutaDocumentoFirmado, BolEmpresaEntidad empresa,string rutaImagen="")
        {
            using (var reader = new PdfReader(rutaDocumentoSinFirma))
            using (var writer = new FileStream(rutaDocumentoFirmado, FileMode.Create, FileAccess.Write))
            using (var stamper = PdfStamper.CreateSignature(reader, writer, '\0', null, true))
            {
                //firma 1
                var signature = stamper.SignatureAppearance;
                signature.CertificationLevel = PdfSignatureAppearance.CERTIFIED_NO_CHANGES_ALLOWED;
                signature.Reason = "Firma del Boletas GDT";
                signature.Location = empresa.emp_depa.ToUpper()+" - "+empresa.emp_pais.ToUpper();
                var signatureKey = new PrivateKeySignature(certificado.Key, DigestAlgorithms.SHA256);
                var signatureChain = certificado.Chain;
                var standard = CryptoStandard.CADES;
             

                StringBuilder buf = new StringBuilder();
                buf.Append("FIRMADO POR: ");
                buf.Append(empresa.emp_nom_rep_legal).Append('\n');
                buf.Append("EMPRESA: ");
                buf.Append(empresa.emp_nomb).Append('\n');
                buf.Append("RUC: ");
                buf.Append(empresa.emp_rucs).Append('\n');
                buf.Append(empresa.emp_depa.ToUpper() + " - " + empresa.emp_pais.ToUpper()).Append('\n');
                buf.Append("Fecha: ").Append(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss zzz"));
                string text = buf.ToString();
                signature.Layer2Text = text;
                if (empresa.emp_firma_visible==1)
                {
                    if (empresa.emp_firma_img != "" && empresa.emp_firma_img!=null)
                    {
                        var imagen = Path.Combine(rutaImagen, empresa.emp_firma_img);
                        if (System.IO.File.Exists(imagen))
                        {
                            var image = iTextSharp.text.Image.GetInstance(imagen);
                            signature.Image = image;
                            signature.Acro6Layers = true;
                        }
                    }
                    signature.SetVisibleSignature(new iTextSharp.text.Rectangle(100, 150, 250, 200), 1, null);
                }
                MakeSignature.SignDetached(signature, signatureKey, signatureChain, null, null, null, 0, standard);
            }
        }
    }
}