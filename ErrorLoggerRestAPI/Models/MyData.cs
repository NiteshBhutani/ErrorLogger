namespace Data
{
    public class MyData
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public MyData()
        {
            this.Id = -1;
            this.Text = string.Empty;
        }

        public MyData(int id, string text)
        {
            this.Id = id;
            this.Text = text;
        }

        public override string ToString()
        {
            return string.Format("Id: {0}, Text: {1}", this.Id, this.Text);
        }
    }
}
