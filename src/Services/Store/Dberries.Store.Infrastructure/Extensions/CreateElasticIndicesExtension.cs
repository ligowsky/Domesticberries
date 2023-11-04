using Nest;

namespace Dberries.Store.Infrastructure;

public static class CreateElasticIndicesExtension
{
    public static void CreateIndices(this IElasticClient client)
    {
        client.Indices.Create("items", index => index
            .Map<Item>(mapping => mapping
                .Properties(props => props
                    .Keyword(keyword => keyword.Name(item => item.Id))
                    .Text(text => text.Name(item => item.Name))
                    .Text(text => text.Name(item => item.Description))
                )
            )
        );
    }
}