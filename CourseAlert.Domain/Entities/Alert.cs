using CourseAlert.Domain.Enums;

namespace CourseAlert.Domain.Entities
{
    public sealed class Alert
    {
        public Guid Id { get; private set; }

        public string BaseCurrency { get; private set; } = null!;

        public string TargetCurrency { get; private set; } = null!;

        public decimal Threshold { get; private set; }

        public AlertDirection Direction { get; private set; }

        public DateTimeOffset CreatedAt { get; private set; }

        // Used by EF Core when materializing the entity from the database.
        private Alert()
        {
        }

        public Alert(
            string baseCurrency,
            string targetCurrency,
            decimal threshold,
            AlertDirection direction)
        {
            BaseCurrency = NormalizeCurrency(baseCurrency, nameof(baseCurrency));
            TargetCurrency = NormalizeCurrency(targetCurrency, nameof(targetCurrency));

            if (BaseCurrency == TargetCurrency)
            {
                throw new ArgumentException(
                    "Base and target currencies must be different.",
                    nameof(targetCurrency));
            }

            if (threshold <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(threshold),
                    threshold,
                    "Threshold must be greater than zero.");
            }

            if (!Enum.IsDefined(direction))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(direction),
                    direction,
                    "Direction is not valid.");
            }

            Id = Guid.NewGuid();
            Threshold = threshold;
            Direction = direction;
            CreatedAt = DateTimeOffset.UtcNow;
        }

        public bool IsTriggered(decimal currentRate)
        {
            if (currentRate <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(currentRate),
                    currentRate,
                    "Current rate must be greater than zero.");
            }

            return Direction switch
            {
                AlertDirection.Above => currentRate > Threshold,
                AlertDirection.Below => currentRate < Threshold,
                _ => throw new InvalidOperationException("Alert direction is invalid.")
            };
        }

        private static string NormalizeCurrency(string currency, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ArgumentException(
                    "Currency is required.",
                    parameterName);
            }

            var normalizedCurrency = currency.Trim().ToUpperInvariant();

            if (normalizedCurrency.Length != 3 ||
                normalizedCurrency.Any(character => character is < 'A' or > 'Z'))
            {
                throw new ArgumentException(
                    "Currency must contain exactly three letters.",
                    parameterName);
            }

            return normalizedCurrency;
        }
    }
}
