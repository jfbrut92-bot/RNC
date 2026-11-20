namespace Utac.Rnc2.MVC.Extensions.mail
{
    public abstract class MailBodyGenarator
    {
        protected MailData _data;

        public MailBodyGenarator()
        {

        }

        public void setData(MailData data)
        {
            this._data = data;
        }

        public virtual string generateSubject()
        {
            string sujet = this._data.TemplateMail.SujetTemplate;

            return sujet;
        }

        public abstract string generateBody();

    }
}
