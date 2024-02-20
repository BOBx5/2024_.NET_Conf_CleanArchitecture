using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySolution.Infrastructure.Persistence.Outbox;
internal class OutboxMessage
{
    public required Guid Id { get; set; }
    public required string Type { get; set; }
    public required string Content { get; set; }
    public required DateTime OccurredOn { get; set; }
    public DateTime? ProcessedOn { get; set; }
    public string? Error { get; set; }
}
