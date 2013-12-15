using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eto.Forms
{
	public partial interface IApplication : IInstanceWidget
	{
		void Attach(object context);

		void Run(string[] args);

		void Quit();

		void GetSystemActions(List<CommandBase> actions, ISubMenuWidget menu, ToolBar toolBar, bool addStandardItems);

		Keys CommonModifier { get; }

		Keys AlternateModifier { get; }

		void Open(string url);

		void Invoke(Action action);

		void AsyncInvoke(Action action);

		string BadgeLabel { get; set; }

		void OnMainFormChanged();
	}

	public partial class Application : InstanceWidget
	{
		public static Application Instance { get; private set; }

		public event EventHandler<EventArgs> Initialized
		{
			add { Properties.AddEvent(InitializedKey, value); }
			remove { Properties.RemoveEvent(InitializedKey, value); }
		}

		static readonly object InitializedKey = new object();

		public virtual void OnInitialized(EventArgs e)
		{
			Properties.TriggerEvent(InitializedKey, this, e);
		}

		public const string TerminatingEvent = "Application.Terminating";

		public event EventHandler<CancelEventArgs> Terminating
		{
			add { Properties.AddHandlerEvent(TerminatingEvent, value); }
			remove { Properties.RemoveEvent(TerminatingEvent, value); }
		}

		public virtual void OnTerminating(CancelEventArgs e)
		{
			Properties.TriggerEvent(TerminatingEvent, this, e);
		}

		new IApplication Handler { get { return (IApplication)base.Handler; } }

		Form mainForm;
		public Form MainForm
		{
			get { return mainForm; }
			set
			{
				mainForm = value;
				Handler.OnMainFormChanged();
			}
		}

		public string Name { get; set; }

		static Application()
		{
			EventLookup.Register<Application>(c => c.OnTerminating(null), Application.TerminatingEvent);
		}

		public Application() : this(Generator.Detect)
		{
		}

		public Application(Generator g) : this(g, typeof(IApplication))
		{
		}

		protected Application(Generator generator, Type type, bool initialize = true)
			: base(generator ?? Generator.Detect, type, initialize)
		{
			Application.Instance = this;
			Generator.Initialize(generator ?? Generator.Detect); // make everything use this by default
		}

		public virtual void Run(params string[] args)
		{
			Handler.Run(args);
		}

		public virtual Application Attach(object context = null)
		{
			Handler.Attach(context);
			return this;
		}

		[Obsolete("Use Invoke instead")]
		public virtual void InvokeOnMainThread(Action action)
		{
			Invoke(action);
		}

		public virtual void Invoke(Action action)
		{
			Handler.Invoke(action);
		}

		public virtual void AsyncInvoke(Action action)
		{
			Handler.AsyncInvoke(action);
		}

		public void Quit()
		{
			Handler.Quit();
		}

		public void Open(string url)
		{
			Handler.Open(url);
		}

		public Keys CommonModifier
		{
			get { return Handler.CommonModifier; }
		}

		public Keys AlternateModifier
		{
			get { return Handler.AlternateModifier; }
		}

		public void GetSystemActions(List<CommandBase> actions, ISubMenuWidget menu, ToolBar toolBar, bool addStandardItems = false)
		{
			Handler.GetSystemActions(actions, menu, toolBar, addStandardItems);
		}

		public string BadgeLabel
		{
			get { return Handler.BadgeLabel; }
			set { Handler.BadgeLabel = value; }
		}
	}
}
