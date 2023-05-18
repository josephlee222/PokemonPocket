namespace PokemonPocket
{
    public class Eevee:Pokemon
    {
        public Eevee(string Name, int Hp, int Exp) : base(Name, Hp, Exp)
        {
            Skill = "Run Away";
            Damage = 20;
        }

        public void calculateDamage()
        {
            this.Hp = this.Hp - (Damage * 2);
        }
    }
}
