namespace Levels {
    public class ResizableBoundedTileEntity<E, C> : BoundedTileEntity<E, C> 
    where E : ResizableBoundedTileEntity<E, C>
    where C : ResizableBoundedTileEntityController<E, C> {
        
    }
}