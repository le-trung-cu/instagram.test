export interface Distance {x: number, y: number}

export function useMousemove(
  options: ({
    onmousedown?: (payload: MouseEvent) => void,
    onmousemove?: (payload: MouseEvent, distance: Distance) => void,
    onmouseup?: (payload: MouseEvent, distance: Distance) => void,
    ontouchstart?: (payload: TouchEvent) => void,
    ontouchmove?: (payload: TouchEvent, distance: Distance) => void,
    ontouchend?: (payload: TouchEvent,distance: Distance) => void
  }) = {}) {
  let mousedown = false;
  let mousedownPosition = {x: 0, y: 0};
  let previousTouchPosition = {x: 0, y: 0};

  function _onmousedown(payload: MouseEvent) {
    mousedown = true;
    mousedownPosition = {x: payload.pageX, y: payload.pageY};
    document.addEventListener('mousemove', _onmousemove);
    document.addEventListener('mouseup', _onmouseup);
    options?.onmousedown?.(payload);
  }
  function _onmousemove(payload: MouseEvent) {
    
    const mousePosition = {x: payload.pageX, y: payload.pageY};
    const distance = {x: mousePosition.x - mousedownPosition.x, y: mousePosition.y - mousedownPosition.y}
    options.onmousemove?.(payload, distance);
  }
  function _onmouseup(payload: MouseEvent) {
    mousedown = false;
    const mouseUpPosition = {x: payload.pageX, y: payload.pageY};
    const distance = {x: mouseUpPosition.x - mousedownPosition.x, y: mouseUpPosition.y - mousedownPosition.y}
    options.onmouseup?.(payload, distance);
    document.removeEventListener('mousemove', _onmousemove);
    document.removeEventListener('mouseup', _onmouseup);
  }

  function _ontouchstart(payload: TouchEvent) {
    mousedown = true;
    mousedownPosition = previousTouchPosition = {x: payload.changedTouches[0].pageX, y: payload.changedTouches[0].pageY};
    document.addEventListener('touchmove', _ontouchmove);
    document.addEventListener('touchend', _ontouchend);
    options.ontouchstart?.(payload);
  }

  function _ontouchend(payload: TouchEvent) {

    mousedown = false;
    const mouseUpPosition = {x: payload.changedTouches[0].pageX, y: payload.changedTouches[0].pageY};
    const distance = {x: mouseUpPosition.x - mousedownPosition.x, y: mouseUpPosition.y - mousedownPosition.y}
    options.ontouchend?.(payload, distance);
    document.removeEventListener('touchmove', _ontouchmove);
    document.removeEventListener('touchend', _ontouchend);
  }

  function _ontouchmove(payload: TouchEvent) {
    const touchPosition = {x: payload.changedTouches[0].pageX, y: payload.changedTouches[0].pageY};
    const distance = {x: touchPosition.x - previousTouchPosition.x, y: touchPosition.y - previousTouchPosition.y};
    options.ontouchmove?.(payload, distance);
  }

  return { onmousedown: _onmousedown, ontouchstart: _ontouchstart }
}