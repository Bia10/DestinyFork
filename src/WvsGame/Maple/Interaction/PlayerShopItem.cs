namespace Destiny.Maple.Interaction
{
    public sealed class PlayerShopItem : Item
    {
        public short Bundles { get; }
        public int MerchantPrice { get; }

        public PlayerShopItem(int mapleID, short bundles, short quantity, int price) : base(mapleID, quantity)
        {
            Bundles = bundles;
            MerchantPrice = price;
        }
    }
}
