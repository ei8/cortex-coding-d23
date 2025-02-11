using ei8.Cortex.Coding.d23.Grannies;
using neurUL.Common.Domain.Model;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization.Processors.Readers.Inductive
{
    public class InstantiatesClassReader : IInstantiatesClassReader
    {
        private readonly IExpressionReader expressionReader;
        private readonly IExternalReferenceSet externalReferences;
        private readonly IAggregateParser aggregateParser;
        private static readonly IGreatGrannyProcess<IInstantiatesClass> target = new GreatGrannyProcess<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                ProcessHelper.TryParse
            );

        public InstantiatesClassReader(
            IExpressionReader expressionReader, 
            IExternalReferenceSet externalReferences,
            IAggregateParser aggregateParser
        )
        {
            AssertionConcern.AssertArgumentNotNull(expressionReader, nameof(expressionReader));
            AssertionConcern.AssertArgumentNotNull(externalReferences, nameof(externalReferences));
            AssertionConcern.AssertArgumentNotNull(aggregateParser, nameof(aggregateParser));

            this.expressionReader = expressionReader;
            this.externalReferences = externalReferences;
            this.aggregateParser = aggregateParser;
        }

        private static IGreatGrannyInfoSuperset<IInstantiatesClass> CreateGreatGrannies(IExpressionReader expressionReader, IInstantiatesClassParameterSet parameters, IExternalReferenceSet externalReferences) =>
            new GreatGrannyInfoSet<IInstantiatesClass>(
               new IGreatGrannyInfo<IInstantiatesClass>[]
               {
                    new InductiveIndependentGreatGrannyInfo<IExpression, IExpressionReader, IExpressionParameterSet, IInstantiatesClass>(
                        parameters.Granny,
                        expressionReader,
                        () => InstantiatesClassReader.CreateSubordinationParameterSet(externalReferences, parameters),
                        (g, r) => r.Class = g.Units.GetValueUnitGranniesByTypeId(externalReferences.DirectObject.Id).Single()
                    )
               },
               InstantiatesClassReader.target
           ).AsSuperset();

        private static ExpressionParameterSet CreateSubordinationParameterSet(IExternalReferenceSet externalReferences, IInstantiatesClassParameterSet parameters) =>
            new ExpressionParameterSet(
                parameters.Granny,
                new[]
                {
                    UnitParameterSet.CreateWithValueAndType(
                        externalReferences.Instantiates,
                        externalReferences.Unit
                    ),
                    UnitParameterSet.CreateWithValueAndType(
                        parameters.Class,
                        externalReferences.DirectObject
                    )
                }
            );

        public bool TryParse(Network network, IInstantiatesClassParameterSet parameters, out IInstantiatesClass result) =>
            this.aggregateParser.TryParse<InstantiatesClass, IInstantiatesClass>(
                parameters.Granny,
                InstantiatesClassReader.CreateGreatGrannies(
                    this.expressionReader, 
                    parameters, 
                    this.externalReferences
                ),
                network,
                out result
            );
    }
}
