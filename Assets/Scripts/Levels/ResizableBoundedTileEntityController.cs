namespace Levels {
    public class ResizableBoundedTileEntityController<E, C> : BoundedTileEntityController<E, C> 
    where E : ResizableBoundedTileEntity<E, C>
    where C : ResizableBoundedTileEntityController<E, C> {
        
    }
}