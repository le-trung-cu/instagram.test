export function whenScrollAtBottom(fn: Function , timeout: number = 100, ...args: any[]) {
  let timeOutId: number | undefined = 0;
  const _fn = async () => {
    if (timeOutId) {
      clearTimeout(timeOutId);
      timeOutId = undefined;
    }
    if ((window.innerHeight + window.pageYOffset + 50) >= document.body.offsetHeight) {
      timeOutId = setTimeout(() => {
        fn(...args)
      }, timeout);
    }
  }
  return {
    listener: () => {
      window.addEventListener('scroll', _fn);
    },
    clearListener: () => {
      window.removeEventListener('scroll', _fn);
    }
  }
}