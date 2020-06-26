using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using Slb.Ocean.Basics;
using Slb.Ocean.Core;
using Slb.Ocean.Geometry;
using Slb.Ocean.Petrel;
using Slb.Ocean.Petrel.DomainObject;
using Slb.Ocean.Petrel.DomainObject.Seismic;
using Slb.Ocean.Petrel.UI;
using Slb.Ocean.Petrel.Workflow;

namespace OceanTestingZone
{
    /// <summary>
    /// This class contains all the methods and subclasses of the MultiThread_SeismicCube.
    /// Worksteps are displayed in the workflow editor.
    /// </summary>
    class MultiThread_SeismicCube : Workstep<MultiThread_SeismicCube.Arguments>, IExecutorSource, IAppearance, IDescriptionSource
    {
        #region Overridden Workstep methods

        /// <summary>
        /// Creates an empty Argument instance
        /// </summary>
        /// <returns>New Argument instance.</returns>

        protected override MultiThread_SeismicCube.Arguments CreateArgumentPackageCore(IDataSourceManager dataSourceManager)
        {
            return new Arguments(dataSourceManager);
        }
        /// <summary>
        /// Copies the Arguments instance.
        /// </summary>
        /// <param name="fromArgumentPackage">the source Arguments instance</param>
        /// <param name="toArgumentPackage">the target Arguments instance</param>
        protected override void CopyArgumentPackageCore(Arguments fromArgumentPackage, Arguments toArgumentPackage)
        {
            DescribedArgumentsHelper.Copy(fromArgumentPackage, toArgumentPackage);
        }

        /// <summary>
        /// Gets the unique identifier for this Workstep.
        /// </summary>
        protected override string UniqueIdCore
        {
            get
            {
                return "2c1dd70c-0d93-4640-b385-ac3f5a84e8e1";
            }
        }
        #endregion

        #region IExecutorSource Members and Executor class

        /// <summary>
        /// Creates the Executor instance for this workstep. This class will do the work of the Workstep.
        /// </summary>
        /// <param name="argumentPackage">the argumentpackage to pass to the Executor</param>
        /// <param name="workflowRuntimeContext">the context to pass to the Executor</param>
        /// <returns>The Executor instance.</returns>
        public Slb.Ocean.Petrel.Workflow.Executor GetExecutor(object argumentPackage, WorkflowRuntimeContext workflowRuntimeContext)
        {
            return new Executor(argumentPackage as Arguments, workflowRuntimeContext);
        }
        
        class OceanSeismicMultithreaded
        {
            public OceanSeismicMultithreaded(int n, int m)
            {
                // Create new output cube
                SeismicCube outputCube = SeismicCube.NullObject;
                SeismicCollection coll = SeismicCollection.NullObject;
                SeismicRoot seisRoot;
                seisRoot = SeismicRoot.Get(PetrelProject.PrimaryProject);
                if (!seisRoot.HasSeismicProject)
                {
                    using (ITransaction tr = DataManager.NewTransaction())
                    {
                        tr.Lock(seisRoot);
                        seisRoot.CreateSeismicProject();
                        tr.Commit();
                    }
                }
                SeismicProject proj = seisRoot.SeismicProject;
                using (ITransaction tr = DataManager.NewTransaction())
                {
                    tr.Lock(proj);
                    coll = proj.CreateSeismicCollection("Test Survey Async " + n.ToString() + "x" + m.ToString());
                    tr.Lock(coll);
                    Index3 size = new Index3(n, n, m);
                    Point3 origin = new Point3(13579.75, 24680.08, 0.0);
                    Vector3 iSpacing = new Vector3(100.0, 0.0, 0.000);
                    Vector3 jSpacing = new Vector3(0.0, 100.0, 0.000);
                    Vector3 kSpacing = new Vector3(0.0, 0.0, -100.0);
                    Index2 annotationOrigin = new Index2(0, 0);
                    Index2 annotationInc = new Index2(1, 1);
                    if (coll.CanCreateSeismicCube(size, origin, iSpacing, jSpacing, kSpacing))
                    {
                        Type dataType = typeof(float);
                        Domain vDomain = Domain.ELEVATION_DEPTH;
                        Template tmpl = PetrelProject.WellKnownTemplates
                        .SeismicColorGroup.SeismicDefault;
                        Range1<double> r = new Range1<double>(0.0, 140.0);
                        _cube = coll.CreateSeismicCube(size, origin, iSpacing, jSpacing, kSpacing, annotationOrigin, annotationInc, dataType, vDomain, tmpl, r);
                    }
                    if (_cube.IsWritable)
                    {
                        MakeCube(_cube);
                    }
                    tr.Commit();
                }
            }


            public OceanSeismicMultithreaded(int n, int m, int decn, int decm, int s)
            {
                // Create new output cube
                SeismicCube outputCube = SeismicCube.NullObject;
                SeismicCollection coll = SeismicCollection.NullObject;
                SeismicRoot seisRoot;
                seisRoot = SeismicRoot.Get(PetrelProject.PrimaryProject);
                if (!seisRoot.HasSeismicProject)
                {
                    using (ITransaction tr = DataManager.NewTransaction())
                    {
                        tr.Lock(seisRoot);
                        seisRoot.CreateSeismicProject();
                        tr.Commit();
                    }
                }
                SeismicProject proj = seisRoot.SeismicProject;
                using (ITransaction tr = DataManager.NewTransaction())
                {
                    tr.Lock(proj);
                    coll = proj.CreateSeismicCollection("Test Survey Async " + n.ToString()+ "x" + m.ToString());
                    tr.Lock(coll);
                    Index3 size = new Index3(n, n, m);
                    Point3 origin = new Point3(13579.75, 24680.08, 0.0);
                    Vector3 iSpacing = new Vector3(100.0, 0.0, 0.000);
                    Vector3 jSpacing = new Vector3(0.0, 100.0, 0.000);
                    Vector3 kSpacing = new Vector3(0.0, 0.0, -100.0);
                    Index2 annotationOrigin = new Index2(0, 0);
                    Index2 annotationInc = new Index2(1, 1);
                    if (coll.CanCreateSeismicCube(size, origin, iSpacing,jSpacing, kSpacing))
                    {
                        Type dataType = typeof(float);
                        Domain vDomain = Domain.ELEVATION_DEPTH;
                        Template tmpl = PetrelProject.WellKnownTemplates
                        .SeismicColorGroup.SeismicDefault;
                        Range1<double> r = new Range1<double>(0.0, 140.0);
                        _cube = coll.CreateSeismicCube(size, origin,iSpacing, jSpacing, kSpacing,annotationOrigin, annotationInc,dataType, vDomain, tmpl, r);
                    }
                    if (_cube.IsWritable)
                    {
                        MakeCube(_cube, decn, decm, s);
                    }
                    tr.Commit();
                }
            }
            private SeismicCube _cube;
            public SeismicCube Cube
            {
                get { return _cube; }
            }
            private class JobSetup
            {
                internal Index3 min;
                internal Index3 max;
                internal int n;
                internal int decn;
                internal int decm;
                internal IAsyncSubCube output;
            };
            private long _threadCounter;
            private long _completedThreads;
            private long _availableWorkers;
            private bool _cancelThread;
            private Object _cancelThreadLock = new Object();
            public void MakeCube(SeismicCube outputCube, int decn, int decm, int n)
            {
                _threadCounter = 0;
                _completedThreads = 0;
                int nwork;
                int compThreads;
                int ncreated = 0;
                ThreadPool.GetMaxThreads(out nwork, out compThreads);
                using (IProgress progress = PetrelLogger.NewProgress(0, 100, ProgressType.Cancelable, Cursors.WaitCursor))
                {
                    // Size of our cubes
                    int ni = outputCube.NumSamplesIJK.I;
                    int nj = outputCube.NumSamplesIJK.J;
                    int nk = outputCube.NumSamplesIJK.K;
                    Index3 cubeSize = outputCube.NumSamplesIJK;
                    var ranges = new Queue<Index3[]>();
                    //Calculate subcube ranges
                    var subsize = new[] { 64, 64, outputCube.NumSamplesIJK.K };
                    for (int i = 0; i < cubeSize.I; i += subsize[0])
                    {
                        for (int j = 0; j < cubeSize.J; j += subsize[1])
                        {
                            for (int k = 0; k < cubeSize.K; k += subsize[2])
                            {
                                var range = new[]
                                {
                                new Index3(i, j, k),
                                new Index3(Math.Min(i + subsize[0] - 1,cubeSize.I - 1),
                                Math.Min(j + subsize[1] - 1, cubeSize.J - 1),
                                Math.Min(k + subsize[2] - 1, cubeSize.K - 1))
                                };
                                ranges.Enqueue(range);
                            }
                        }
                    }
                    int numWorkers = (256 * 1024 * 1024 / 4) / (subsize[0] * subsize[1] * subsize[2]);
                    ThreadPool.SetMaxThreads(numWorkers, numWorkers);
                    _availableWorkers = numWorkers;
                    int numRanges = ranges.Count;
                    ThreadPool.GetMaxThreads(out nwork, out compThreads);
                    PetrelLogger.InfoOutputWindow("nwork= " + nwork +" comp = " + compThreads);
                    int scheduledTasks = 0;
                    int percent;
                    _cancelThread = false;
                    while (ranges.Count > 0)
                    {
                        using (ITransaction trans =DataManager.NewTransaction())
                        {
                            if (progress.IsCanceled)
                            {
                                lock (_cancelThreadLock)
                                {
                                    _cancelThread = true;
                                }
                                outputCube.Delete();
                                break;
                            }
                            Application.DoEvents();
                            while (ranges.Count > 0 &&Interlocked.Read(ref _availableWorkers) > 0)
                            {
                                Index3[] range = ranges.Dequeue();
                                Index3 minIJK = range[0];
                                Index3 maxIJK = range[1];
                                var jobSetup = new JobSetup();
                                jobSetup.min = range[0];
                                jobSetup.max = range[1];
                                jobSetup.decn = decn;
                                jobSetup.decm = decm;
                                jobSetup.n = n;
                                trans.Lock(outputCube);
                                jobSetup.output =outputCube.GetAsyncSubCubeReadWrite(minIJK, maxIJK);
                                //Using thread pool to run the jobs.
                                Interlocked.Increment(ref _threadCounter);
                                Interlocked.Decrement(ref _availableWorkers);
                                ThreadPool.QueueUserWorkItem(ProcessOneRange, jobSetup);
                                ncreated++;
                                PetrelLogger.InfoOutputWindow("n=" + ncreated);
                                scheduledTasks++;
                            }
                            // **********************************
                            // Wait for the entire batch of threads to finish
                            // before spinning up new threads.
                            // **********************************
                            while (Interlocked.Read(ref _completedThreads)< Interlocked.Read(ref _threadCounter))
                            {
                                percent = (int)((scheduledTasks -(Interlocked.Read(ref _threadCounter) -Interlocked.Read(ref _completedThreads)))* 100f / numRanges);
                                progress.ProgressStatus = percent;
                                if (progress.IsCanceled)
                                {
                                    lock (_cancelThreadLock)
                                    {
                                        _cancelThread = true;
                                    }
                                    outputCube.Delete();
                                    break;
                                }
                                Application.DoEvents();
                                Thread.Sleep(100);
                                System.GC.Collect();
                            }
                            if (_cancelThread) break;
                            trans.Commit();
                        }
                    }
                }
            }
            public void MakeCube(SeismicCube outputCube)
            {
                _threadCounter = 0;
                _completedThreads = 0;
                int nwork;
                int compThreads;
                int ncreated = 0;
                ThreadPool.GetMaxThreads(out nwork, out compThreads);
                using (IProgress progress = PetrelLogger.NewProgress(0, 100, ProgressType.Cancelable, Cursors.WaitCursor))
                {
                    // Size of our cubes
                    int ni = outputCube.NumSamplesIJK.I;
                    int nj = outputCube.NumSamplesIJK.J;
                    int nk = outputCube.NumSamplesIJK.K;
                    Index3 cubeSize = outputCube.NumSamplesIJK;
                    var ranges = new Queue<Index3[]>();
                    //Calculate subcube ranges
                    var subsize = new[] { 64,64, outputCube.NumSamplesIJK.K };
                    for (int i = 0; i < cubeSize.I; i += subsize[0])
                    {
                        for (int j = 0; j < cubeSize.J; j += subsize[1])
                        {
                            for (int k = 0; k < cubeSize.K; k += subsize[2])
                            {
                                var range = new[]
                                {
                                new Index3(i, j, k),
                                new Index3(Math.Min(i + subsize[0] - 1,cubeSize.I - 1),
                                Math.Min(j + subsize[1] - 1, cubeSize.J - 1),
                                Math.Min(k + subsize[2] - 1, cubeSize.K - 1))
                                };
                                ranges.Enqueue(range);
                            }
                        }
                    }
                    int numWorkers = (256 * 1024 * 1024 / 4) / (subsize[0] * subsize[1] * subsize[2]);
                    bool val=ThreadPool.SetMaxThreads(numWorkers, numWorkers);
                    PetrelLogger.InfoOutputWindow("SetMaxThreads= " + val);
                    _availableWorkers = numWorkers;
                    int numRanges = ranges.Count;
                    ThreadPool.GetMaxThreads(out nwork, out compThreads);
                    PetrelLogger.InfoOutputWindow("nwork= " + nwork + " comp = " + compThreads);
                    int scheduledTasks = 0;
                    int percent;
                    _cancelThread = false;
                    while (ranges.Count > 0)
                    {
                        using (ITransaction trans = DataManager.NewTransaction())
                        {
                            if (progress.IsCanceled)
                            {
                                lock (_cancelThreadLock)
                                {
                                    _cancelThread = true;
                                }
                                outputCube.Delete();
                                break;
                            }
                            Application.DoEvents();
                            while (ranges.Count > 0 && Interlocked.Read(ref _availableWorkers) > 0)
                            {
                                Index3[] range = ranges.Dequeue();
                                Index3 minIJK = range[0];
                                Index3 maxIJK = range[1];
                                var jobSetup = new JobSetup();
                                jobSetup.min = range[0];
                                jobSetup.max = range[1];
                                jobSetup.decn = 0;
                                jobSetup.decm = 0;
                                jobSetup.n = 0;
                                trans.Lock(outputCube);
                                jobSetup.output = outputCube.GetAsyncSubCubeReadWrite(minIJK, maxIJK);
                                //Using thread pool to run the jobs.
                                Interlocked.Increment(ref _threadCounter);
                                Interlocked.Decrement(ref _availableWorkers);
                                ThreadPool.QueueUserWorkItem(ProcessOneRange, jobSetup);
                                ncreated++;
                                PetrelLogger.InfoOutputWindow("n=" + ncreated);
                                scheduledTasks++;
                            }
                            // **********************************
                            // Wait for the entire batch of threads to finish
                            // before spinning up new threads.
                            // **********************************
                            while (Interlocked.Read(ref _completedThreads) < Interlocked.Read(ref _threadCounter))
                            {
                                percent = (int)((scheduledTasks - (Interlocked.Read(ref _threadCounter) - Interlocked.Read(ref _completedThreads))) * 100f / numRanges);
                                progress.ProgressStatus = percent;
                                if (progress.IsCanceled)
                                {
                                    lock (_cancelThreadLock)
                                    {
                                        _cancelThread = true;
                                    }
                                    outputCube.Delete();
                                    break;
                                }
                                Application.DoEvents();
                                Thread.Sleep(1000);
                                System.GC.Collect();
                            }
                            if (_cancelThread) break;
                            trans.Commit();
                        }
                    }
                }
            }
            public void ProcessOneRange(object state)
            {
                JobSetup jobSetup = (JobSetup)state;
                Index3 min = jobSetup.output.MinIJK;
                Index3 max = jobSetup.output.MaxIJK;
                Index3 size = max - min + 1;
                int ni = size.I;
                int nj = size.J;
                int nk = size.K;
                try
                {
                    Index3 idx = new Index3();
                    PetrelLogger.InfoOutputWindow("ToArray " +min.ToString() + "," + max.ToString());
                    float[,,] data = null;
                    lock (jobSetup)
                    {
                        data = jobSetup.output.ToArray();
                    }
                    PetrelLogger.InfoOutputWindow("Setting " +min.ToString() + "," + max.ToString());
                    Random rd = new Random();
                    for (int i = 0; i < ni; i++)
                    {
                        idx.I = min.I + i;
                        for (int j = 0; j < nj; j++)
                        {
                            idx.J = min.J + j;
                            for (int k = 0; k < nk; k++)
                            {
                                idx.K = min.K + k;                                
                                float a= (float)rd.NextDouble() * 10000;
                                data[idx.I - min.I, idx.J - min.J, idx.K - min.K] = a;
                                //data[idx.I, idx.J, idx.K] = a;
                            }
                        }
                        
                        //Interlocked.Increment(ref _availableWorkers);
                        //Interlocked.Increment(ref _completedThreads);
                        //data = null;
                        //GC.Collect();
                    }
                    bool canceled;
                    lock (_cancelThreadLock)
                    {
                        canceled = _cancelThread;
                    }
                    lock (jobSetup)
                    {
                        if (jobSetup.output != null)
                        {
                            if (!canceled)
                            {
                                PetrelLogger.InfoOutputWindow("CopyFrom " + min.ToString() + "," + max.ToString());
                                jobSetup.output.CopyFrom(data);
                                jobSetup.output.Dispose();
                                PetrelLogger.InfoOutputWindow("Disposed ");
                                PetrelLogger.InfoOutputWindow("Committed");
                            }
                        }
                    }
                    Interlocked.Increment(ref _availableWorkers);
                    Interlocked.Increment(ref _completedThreads);
                    data = null;
                    GC.Collect();
                }
                catch (Exception e)
                {
                    PetrelLogger.InfoOutputWindow("Exception in thread " +min.ToString() + "," + max.ToString() + ": " + e.Message);
                    Interlocked.Increment(ref _availableWorkers);
                    Interlocked.Increment(ref _completedThreads);
                }
            }
        }
           
    public class Executor : Slb.Ocean.Petrel.Workflow.Executor
    {
        Arguments arguments;
        WorkflowRuntimeContext context;

        public Executor(Arguments arguments, WorkflowRuntimeContext context)
        {
            this.arguments = arguments;
            this.context = context;
        }

        public override void ExecuteSimple()
        {
            //OceanSeismicMultithreaded os = new OceanSeismicMultithreaded(arguments.TracesIJ, arguments.NSamples, arguments.BodySpacingIJ, arguments.BodySpacingSamples, arguments.BodySize);
            OceanSeismicMultithreaded os = new OceanSeismicMultithreaded(arguments.TracesIJ, arguments.NSamples);
        }
  
    }


        #endregion

        /// <summary>
        /// ArgumentPackage class for MultiThread_SeismicCube.
        /// Each public property is an argument in the package.  The name, type and
        /// input/output role are taken from the property and modified by any
        /// attributes applied.
        /// </summary>
        public class Arguments : DescribedArgumentsByReflection
        {
            public Arguments()
                : this(DataManager.DataSourceManager)
            {                
            }

            public Arguments(IDataSourceManager dataSourceManager)
            {
            }

            private int tracesIJ;
            private int nSamples;
            private int bodySpacingIJ;
            private int bodySpacingSamples;
            private int bodySize;

            [Description("TracesIJ", "Num traces in I and J dimensions")]
            public int TracesIJ
            {
                internal get { return this.tracesIJ; }
                set { this.tracesIJ = value; }
            }
            [Description("NSamples", "Samples per trace")]
            public int NSamples
            {
                internal get { return this.nSamples; }
                set { this.nSamples = value; }
            }
            //[Description("BodySpacingIJ", "Distance btw geobodies in I and J")]
            //public int BodySpacingIJ
            //{
            //    internal get { return this.bodySpacingIJ; }
            //    set { this.bodySpacingIJ = value; }
            //}
            //[Description("BodySpacingSamples","Distance between geobodies in samples")]
            //public int BodySpacingSamples
            //{
            //    internal get { return this.bodySpacingSamples; }
            //    set { this.bodySpacingSamples = value; }
            //}
            //[Description("BodySize","Size of geobody spheres in samples")]
            //public int BodySize
            //{
            //    internal get { return this.bodySize; }
            //    set { this.bodySize = value; }
            //}


        }
    
        #region IAppearance Members
        public event EventHandler<TextChangedEventArgs> TextChanged;
        protected void RaiseTextChanged()
        {
            if (this.TextChanged != null)
                this.TextChanged(this, new TextChangedEventArgs(this));
        }

        public string Text
        {
            get { return Description.Name; }
            private set 
            {
                // TODO: implement set
                this.RaiseTextChanged();
            }
        }

        public event EventHandler<ImageChangedEventArgs> ImageChanged;
        protected void RaiseImageChanged()
        {
            if (this.ImageChanged != null)
                this.ImageChanged(this, new ImageChangedEventArgs(this));
        }

        public System.Drawing.Bitmap Image
        {
            get { return PetrelImages.Modules; }
            private set 
            {
                // TODO: implement set
                this.RaiseImageChanged();
            }
        }
        #endregion

        #region IDescriptionSource Members

        /// <summary>
        /// Gets the description of the MultiThread_SeismicCube
        /// </summary>
        public IDescription Description
        {
            get { return MultiThread_SeismicCubeDescription.Instance; }
        }

        /// <summary>
        /// This singleton class contains the description of the MultiThread_SeismicCube.
        /// Contains Name, Shorter description and detailed description.
        /// </summary>
        public class MultiThread_SeismicCubeDescription : IDescription
        {
            /// <summary>
            /// Contains the singleton instance.
            /// </summary>
            private  static MultiThread_SeismicCubeDescription instance = new MultiThread_SeismicCubeDescription();
            /// <summary>
            /// Gets the singleton instance of this Description class
            /// </summary>
            public static MultiThread_SeismicCubeDescription Instance
            {
                get { return instance; }
            }

            #region IDescription Members

            /// <summary>
            /// Gets the name of MultiThread_SeismicCube
            /// </summary>
            public string Name
            {
                get { return "MultiThread_SeismicCube"; }
            }
            /// <summary>
            /// Gets the short description of MultiThread_SeismicCube
            /// </summary>
            public string ShortDescription
            {
                get { return ""; }
            }
            /// <summary>
            /// Gets the detailed description of MultiThread_SeismicCube
            /// </summary>
            public string Description
            {
                get { return ""; }
            }

            #endregion
        }
        #endregion


    }
}