namespace SimpleEmailService.DataAccess.Entities
{
    public class Contact
    {
        public long Id { get; set; }

        public string Name { get; set; } = null!;

        public DateOnly? BirthDate { get; set; } = null!;

        public ICollection<Email> Emails { get; set; } = null!;

		#region Equality and Hashcode

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return Equals((Contact)obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = Id.GetHashCode();
				hashCode = Name?.GetHashCode() ?? 0;
				hashCode = BirthDate?.GetHashCode() ?? 0;
				hashCode = Emails?.GetHashCode() ?? 0;
				hashCode = (hashCode * 17);
				return hashCode;
			}
		}

		protected bool Equals(Contact other)
		{
			return Id == other.Id &&
				   Name == other.Name &&
				   BirthDate == other.BirthDate &&
				   Emails != null && other.Emails != null &&
				   Emails.SequenceEqual(other.Emails);
		}

		public override string ToString()
        {
			return $"Id: {Id}, Name: {Name}, BirthDate: {BirthDate}, Email Count: {Emails.Count()}";
        }

		#endregion
	}
}
