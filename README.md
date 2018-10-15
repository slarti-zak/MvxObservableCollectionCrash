# MvxObservableCollection Crash Showcase

This demo app demonstrates a crash in the MvxObservableCollection in regards to thread unsafety.

## App

This app randomly removes and adds items on a background thread into a MvxObservableCollection. A unique id is assigned to each item.

## Reproduce the Crash

- Start the App
- Start the modification of the list entries using the button at the top
- Updates start happening on a separate thread
- Keep scrolling to the bottom of the list until the crash happens

## Bugs

### Crash

The following Crash happens on Android:

```csharp
Java.Lang.IndexOutOfBoundsException: Inconsistency detected. Invalid view holder adapter positionViewHolder{3dde0bb position=48 id=-1, oldPos=-1, pLpos:-1 scrap [attachedScrap] tmpDetached not recyclable(1) no parent} mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView{f372d18 VFED..... .F....ID 0,126-1080,1731 #7f0a009d app:id/recyclerview}, adapter:mvvmcross.droid.support.v7.recyclerview.MvxRecyclerAdapter@9f978fb, layout:mvvmcross.droid.support.v7.recyclerview.MvxGuardedLinearLayoutManager@bce0556, context:md51ebd907b5225b7e06206587054b74677.MainActivity@bf554c3
  at java.lang.IndexOutOfBoundsException: Inconsistency detected. Invalid view holder adapter positionViewHolder{3dde0bb position=48 id=-1, oldPos=-1, pLpos:-1 scrap [attachedScrap] tmpDetached not recyclable(1) no parent} mvvmcross.droid.support.v7.recyclerview.MvxRecyclerView{f372d18 VFED..... .F....ID 0,126-1080,1731 #7f0a009d app:id/recyclerview}, adapter:mvvmcross.droid.support.v7.recyclerview.MvxRecyclerAdapter@9f978fb, layout:mvvmcross.droid.support.v7.recyclerview.MvxGuardedLinearLayoutManager@bce0556, context:md51ebd907b5225b7e06206587054b74677.MainActivity@bf554c3
  at at android.support.v7.widget.RecyclerView$Recycler.validateViewHolderForOffsetPosition(RecyclerView.java:5447)
  at at android.support.v7.widget.RecyclerView$Recycler.tryGetViewHolderForPositionByDeadline(RecyclerView.java:5629)
  at at android.support.v7.widget.RecyclerView$Recycler.getViewForPosition(RecyclerView.java:5589)
  at at android.support.v7.widget.RecyclerView$Recycler.getViewForPosition(RecyclerView.java:5585)
  at at android.support.v7.widget.LinearLayoutManager$LayoutState.next(LinearLayoutManager.java:2231)
  at at android.support.v7.widget.LinearLayoutManager.layoutChunk(LinearLayoutManager.java:1558)
  at at android.support.v7.widget.LinearLayoutManager.fill(LinearLayoutManager.java:1518)
  at at android.support.v7.widget.LinearLayoutManager.scrollBy(LinearLayoutManager.java:1332)
  at at android.support.v7.widget.LinearLayoutManager.scrollVerticallyBy(LinearLayoutManager.java:1076)
  at at android.support.v7.widget.RecyclerView$ViewFlinger.run(RecyclerView.java:4864)
  at at android.view.Choreographer$CallbackRecord.run(Choreographer.java:949)
  at at android.view.Choreographer.doCallbacks(Choreographer.java:761)
  at at android.view.Choreographer.doFrame(Choreographer.java:693)
  at at android.view.Choreographer$FrameDisplayEventReceiver.run(Choreographer.java:935)
  at at android.os.Handler.handleCallback(Handler.java:873)
  at at android.os.Handler.dispatchMessage(Handler.java:99)
  at at android.os.Looper.loop(Looper.java:193)
  at at android.app.ActivityThread.main(ActivityThread.java:6669)
  at at java.lang.reflect.Method.invoke(Native Method)
  at at com.android.internal.os.RuntimeInit$MethodAndArgsCaller.run(RuntimeInit.java:493)
  at at com.android.internal.os.ZygoteInit.main(ZygoteInit.java:858)

```

This is due to the fact, that the MvxObservableCollection gets updated while the view accesses the collection to render the updated entries. Since the list is not locked, the view may want to access an item, that is no longer in the collection.

### Incorrect display

The fact that the Collection gets updated in the background may also result in incorrectly displayed items.
![Item Duplicates](MvxObservableCollection.png)