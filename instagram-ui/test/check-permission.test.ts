import {checkPermission} from '../src/components/common/permission/check-permission';
import { test, describe, it, expect,assert } from "vitest";

describe('can view saved post', () => {
  it('', async () => {
    expect(await checkPermission('canViewSavedPost', {userName: 'a'}, {userName: 'a'})).toBe(true)
    expect(await checkPermission('canViewSavedPost', {userName: 'b'}, {userName: 'v'})).toBe(false)
    // expect(checkPermission('canViewSavedPost', {userName: 'a'}, {userName: 'a'})).toBe(true)
    // expect(checkPermission('canViewSavedPost', {userName: 'a', id: 'a'}, 'a')).toBe(true)
  })
})
