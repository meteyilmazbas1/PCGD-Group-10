
namespace UrbanNinja
{
    public interface IPickUpTaker
    {
        public bool CanTake(Pickup pickup);
        public void TakeWeapon(Weapon weapon);
        public void TakeHealth(int amount);
        public void TakeScore(int amount);
    }
}