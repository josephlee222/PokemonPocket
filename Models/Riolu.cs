namespace PokemonPocket
{
    public class Riolu:Pokemon
    {
        public Riolu(int Hp, int Exp) : base(Hp, Exp)
        {
            Name = "Riolu";
            Skill = "Prankster";
            Damage = 25;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 2);
        }
    }
}
