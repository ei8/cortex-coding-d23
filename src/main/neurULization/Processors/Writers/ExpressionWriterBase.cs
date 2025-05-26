using ei8.Cortex.Coding.d23.Grannies;
using System.Collections.Generic;
using System;
using ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Deductive;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Writers
{
    public abstract class ExpressionWriterBase<TExpressionReader, TExpressionParameterSet> :
        ILesserGrannyWriter<IExpression, TExpressionParameterSet>
        where TExpressionReader : ILesserGrannyReader<IExpression, TExpressionParameterSet>
        where TExpressionParameterSet : IExpressionParameterSet
    {
        internal const string ExpressionTypePostsynapticInfoName = "ExpressionType";
        private readonly IUnitWriter unitWriter;
        private readonly TExpressionReader reader;
        private readonly IMirrorSet mirrors;

        public ExpressionWriterBase(
            IUnitWriter unitWriter,
            TExpressionReader reader,
            IMirrorSet mirrors
        )
        {
            this.unitWriter = unitWriter;
            this.reader = reader;
            this.mirrors = mirrors;
        }

        protected abstract Neuron CreateGrannyNeuron(TExpressionParameterSet parameters);

        public bool TryBuild(Network network, TExpressionParameterSet parameters, out IExpression result) =>
            this.TryBuildAggregate(
                () => new Expression(),
                parameters,
                parameters.UnitParameters.Select(
                    u => new GreatGrannyProcess<IUnit, IUnitWriter, IUnitParameterSet, IExpression>(
                        ProcessHelper.TryParseBuild
                    )
                ),
                network,
                this.mirrors,
                out result,
                () => network.AddOrGetIfExists(
                    this.CreateGrannyNeuron(parameters)
                ),
                (r) => r.ToPostsynapticInfos(r.Units, g => r.Units)
            );

        public bool TryCreateGreatGrannies(
            TExpressionParameterSet parameters,
            Network network,
            IMirrorSet mirrors,
            out IEnumerable<IGreatGrannyInfo<IExpression>> result
        ) => this.TryCreateGreatGranniesCore(
            delegate (out bool bResult) {
                bResult = true;
                var coreBResult = true;
                var coreResult = parameters.UnitParameters.Select(
                    u => new IndependentGreatGrannyInfo<IUnit, IUnitWriter, IUnitParameterSet, IExpression>(
                        this.unitWriter,
                        () => u,
                        (g, r) => r.Units.Add(g)
                    )
                );
                bResult = coreBResult;
                return coreResult;
            },
            out result
        );

        public Readers.Deductive.IGrannyReader<IExpression, TExpressionParameterSet> Reader => this.reader;
    }
}
