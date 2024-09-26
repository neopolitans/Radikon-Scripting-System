namespace Radikon.ScriptingSystem
{
    /// <summary>
    /// <see langword="RDKCore:"/> An interface to inherit to guarantee a node has a finalize method. <br/>
    /// This could be used for destroying any temporary mission object behaviour or removing any global-scope variables from the global variables collection.
    /// </summary>
    public interface IFinalizeMethodProvider
    {
        public void FinalizeNode();
    }

}