namespace Esacco.Helpers
{
    public class CustomGridItemsLayout : GridItemsLayout
    {
        public CustomGridItemsLayout() : base(1, ItemsLayoutOrientation.Vertical)
        {
        }

        public void UpdateSpan(int itemCount)
        {
            if (itemCount == 0)
            {
                this.Span = 1;
            }
            else if (itemCount == 1)
            {
                this.Span = 1;
            }
            else if (itemCount == 2 || itemCount == 4)
            {
                this.Span = 2;
            }
            else if (itemCount == 3 || itemCount == 5)
            {
                this.Span = 3;
            }
            else
            {
                this.Span = 4; // Default span for more items
            }
        }
    }
}
