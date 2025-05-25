using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Queries;
using System.Collections.Generic;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IMirrorSet mirrors;

        public InstantiatesClassReader(IExpressionReader expressionReader, IMirrorSet mirrors)
        {
            this.expressionReader = expressionReader;
            this.mirrors = mirrors;
        }

        public bool TryCreateGreatGrannies(
            IInstantiatesClassParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out IEnumerable<IGreatGrannyInfo<IInstantiatesClass>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
            bResult = true;
            var coreBResult = true;
            var coreResult = new IGreatGrannyInfo<IInstantiatesClass>[]
                {
                    new IndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        expressionReader,
                        () => InstantiatesClassReader.CreateSubordinationParameterSet(mirrors, parameters),
                        (g, r) => {
                            if (coreBResult = g.TryGetValueUnitGrannyByTypeId(mirrors.DirectObject.Id, out IUnit vuResult))
                                r.Class = vuResult;
                        }
                    )
                };
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        public IEnumerable<IGrannyQuery> GetQueries(Network network, IInstantiatesClassParameterSet parameters) =>
            this.expressionReader.GetQueries(network,
                InstantiatesClassReader.CreateSubordinationParameterSet(this.mirrors, parameters)
            );

        private static ExpressionParameterSet CreateSubordinationParameterSet(IMirrorSet mirrors, IInstantiatesClassParameterSet parameters)
        {
            return new ExpressionParameterSet(
                new[]
                {
                    new UnitParameterSet(
                        mirrors.Instantiates,
                        mirrors.Unit
                    ),
                    new UnitParameterSet(
                        parameters.Class,
                        mirrors.DirectObject
                    )
                }
            );
        }

        public bool TryParse(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.TryParseAggregate(
                () => new InstantiatesClass(),
                parameters,
                new IGreatGrannyProcess<IInstantiatesClass>[]
                {
                    new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        ProcessHelper.TryParse
                    )
                },
                network,
                this.mirrors,
                out result
            );
    }
}
