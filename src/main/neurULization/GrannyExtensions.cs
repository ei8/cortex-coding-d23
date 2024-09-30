using ei8.Cortex.Coding.d23.Grannies;
using ei8.Cortex.Coding.d23.neurULization.Selectors;
using Nancy.TinyIoc;
using neurUL.Common.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ei8.Cortex.Coding.d23.neurULization
{
    public static class GrannyExtensions
    {
        internal static void TryParseCore<TGranny>(
            this TGranny granny,
            Ensemble ensemble,
            IEnumerable<Guid> selection,
            LevelParser[] levelParsers,
            Action<Neuron> resultProcessor,
            ref TGranny result,
            bool throwExceptionOnRedundantData = true
        )
            where TGranny : IGranny
        {
            foreach (var levelParser in levelParsers)
                selection = levelParser.Evaluate(ensemble, selection);

            if (selection.Any())
            {
                if (throwExceptionOnRedundantData)
                    AssertionConcern.AssertStateTrue(
                        selection.Count() == 1,
                        $"Redundant items encountered while parsing ensemble: {string.Join(", ", selection)}"
                        );

                if (ensemble.TryGetById(selection.Single(), out Neuron ensembleResult))
                {
                    resultProcessor(ensembleResult);
                    result = granny;
                }
            }
        }

        public static void AddWriters(this TinyIoCContainer container)
        {
            container.Register<
                Processors.Writers.IExpressionWriter, 
                Processors.Writers.ExpressionWriter
            >();
            container.Register<
                Processors.Writers.IInstantiatesClassWriter, 
                Processors.Writers.InstantiatesClassWriter
            >();
            container.Register<
                Processors.Writers.IPropertyAssignmentWriter, 
                Processors.Writers.PropertyAssignmentWriter
            >();
            container.Register<
                Processors.Writers.IPropertyAssociationWriter, 
                Processors.Writers.PropertyAssociationWriter
            >();
            container.Register<
                Processors.Writers.IPropertyValueExpressionWriter, 
                Processors.Writers.PropertyValueExpressionWriter
            >();
            container.Register<
                Processors.Writers.IUnitWriter, 
                Processors.Writers.UnitWriter
            >();
            container.Register<
                Processors.Writers.IValueWriter, 
                Processors.Writers.ValueWriter
            >();
            container.Register<
                Processors.Writers.IValueExpressionWriter, 
                Processors.Writers.ValueExpressionWriter
            >();
            container.Register<
                Processors.Writers.IInstanceWriter, 
                Processors.Writers.InstanceWriter
            >();
        }

        public static void AddReaders(this TinyIoCContainer container)
        {
            container.Register<
                Processors.Readers.Inductive.IExpressionReader,
                Processors.Readers.Inductive.ExpressionReader
            >();
            container.Register<
                Processors.Readers.Inductive.IInstantiatesClassReader,
                Processors.Readers.Inductive.InstantiatesClassReader
            >();
            container.Register<
                Processors.Readers.Inductive.IPropertyAssignmentReader,
                Processors.Readers.Inductive.PropertyAssignmentReader
                >();
            container.Register<
                Processors.Readers.Inductive.IPropertyAssociationReader,
                Processors.Readers.Inductive.PropertyAssociationReader
                >();
            container.Register<
               Processors.Readers.Inductive.IPropertyValueExpressionReader,
               Processors.Readers.Inductive.PropertyValueExpressionReader
            >();
            container.Register<
                Processors.Readers.Inductive.IUnitReader,
                Processors.Readers.Inductive.UnitReader
            >();
            container.Register<
                Processors.Readers.Inductive.IValueReader,
                Processors.Readers.Inductive.ValueReader
            >();
            container.Register<
                Processors.Readers.Inductive.IValueExpressionReader,
                Processors.Readers.Inductive.ValueExpressionReader
            >();
            container.Register<
                Processors.Readers.Inductive.IInstanceReader,
                Processors.Readers.Inductive.InstanceReader
            >();
        }
    }
}
