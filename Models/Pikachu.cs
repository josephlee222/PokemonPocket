namespace PokemonPocket
{
    public class Pikachu:Pokemon
    {
        public Pikachu(string Name, int Hp, int Exp): base(Name, Hp, Exp)
        {
            Skill = "Lightning Bolt";
            Damage = 25;
        }

        public void calculateDamage()
        {
            this.Hp = this.Hp - (Damage * 1);
        }
    }
}
