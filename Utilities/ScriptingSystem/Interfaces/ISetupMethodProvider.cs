namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> An interface to inherit to guarantee a node has a setup method. <br/>
    /// This could be used for creating any temporary mission object behaviour or creatomg any global-scope variables in the global variables collection.
    /// </summary>
    public interface ISetupMethodProvider
    {
        public void SetupNode();
    }

}