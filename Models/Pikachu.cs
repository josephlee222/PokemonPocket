namespace PokemonPocket
{
    public class Pikachu:Pokemon
    {
        public Pikachu(int Hp, int Exp): base(Hp, Exp)
        {
            Name = "Pikachu";
            Skill = "Lightning Bolt";
            Damage = 25;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 1);
        }
    }
}
