using Zengenti.Contensis.Delivery;


namespace RazorPageCampaignsWebsite.Core.Interfaces
{


    public interface IContensisClient
    {
        NodeOperations Nodes { get; }
        EntryOperations Entries { get; }
    }

    public class ContensisClientWrapper : IContensisClient
    {
        private readonly ContensisClient _inner;

        public ContensisClientWrapper(ContensisClient inner)
        {
            _inner = inner;
        }

        public NodeOperations Nodes => _inner.Nodes;
        public EntryOperations Entries => _inner.Entries;
    }

}
