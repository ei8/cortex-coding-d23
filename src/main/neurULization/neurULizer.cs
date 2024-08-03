using System.Collections.Generic;
using System.Threading.Tasks;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public class neurULizer : IneurULizer
    {
        public async Task<Ensemble> neurULizeAsync<TValue, TOptions>(TValue value, TOptions options)
            where TOptions : IneurULizerWriteOptions
            => await this.neurULizeAsync<TValue>(value, (Id23neurULizerWriteOptions) options);

        public IEnumerable<TValue> DeneurULize<TValue, TOptions>(Ensemble value, TOptions options)
            where TValue : class, new()
            where TOptions : IneurULizerReadOptions
            => this.DeneurULize<TValue>(value, (Id23neurULizerReadOptions) options);
    }
}
