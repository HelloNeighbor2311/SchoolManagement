namespace SchoolManagement.Models
{
    public enum Rank
    {
        Excellent,
        Good,
        Average,
        Bad
    }
    public class Gpa
    {
        public int GPAId { get; set; }
        public int StudentId { get; set; }
        public int SemesterId { get; set; }
        public int TotalCredits { get; set; }
        public double? gpa { get; set; } = null;
        public Rank? rank { get; set; }

        public Semester? Semester { get; set; }
        public Student? Student { get; set; }
        public List<Award>? Award { get; set; }
        public void SetRank(double? value)
        {
            rank = value switch
            {
                >= 3.6 => Models.Rank.Excellent,
                >= 3.2 and < 3.6 => Models.Rank.Good,
                >= 2.5 and < 3.2 => Models.Rank.Average,
                >= 1.9 and < 2.5 => Rank.Bad,
                _ => null
            };
        }
    }
}
