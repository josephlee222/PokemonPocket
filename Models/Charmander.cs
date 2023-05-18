namespace PokemonPocket
{
    public class Charmander : Pokemon
    {
        public Charmander(string Name, int Hp, int Exp) : base(Name, Hp, Exp)
        {
            Skill = "Solar Power";
            Damage = 15;
        }

        public void calculateDamage()
        {
            this.Hp = this.Hp - (Damage * 3);
        }
    }
}
