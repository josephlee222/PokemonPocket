namespace PokemonPocket
{
    public class Pachirisu:Pokemon
    {
        public Pachirisu(int Hp, int Exp) : base(Hp, Exp)
        {
            Name = "Pachirisu";
            Skill = "Volt Absorb";
            Damage = 15;
        }

        public void calculateDamage(int damage_taken)
        {
            this.Hp = this.Hp - (damage_taken * 1);
        }
    }
}
