package md5f90650d7b1504366e4a43a746a3afba1;


public class Configurar
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_btnSalvarClicked_Click:(Landroid/view/View;)V:__export__\n" +
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("AppDescansoBiblia.Configurar, AppDescansoBiblia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Configurar.class, __md_methods);
	}


	public Configurar () throws java.lang.Throwable
	{
		super ();
		if (getClass () == Configurar.class)
			mono.android.TypeManager.Activate ("AppDescansoBiblia.Configurar, AppDescansoBiblia, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void btnSalvarClicked (android.view.View p0)
	{
		n_btnSalvarClicked_Click (p0);
	}

	private native void n_btnSalvarClicked_Click (android.view.View p0);


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
